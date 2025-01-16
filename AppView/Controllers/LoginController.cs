using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
	public class LoginController : Controller
	{
		private readonly HotelDbContext _context;

		public LoginController(HotelDbContext context)
		{
			_context = context;
		}

		// GET: Login
		public IActionResult Login()
		{
			if (HttpContext.Session.GetString("TaiKhoan") != null)
			{
				return RedirectToAction("Index", "AdminDashboard", new { area = "Admin" });
			}
			return View();
		}

		// Xử lý đăng nhập
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Tìm tài khoản trong cơ sở dữ liệu
				var user = await _context.TaiKhoans
					.FirstOrDefaultAsync(t => t.TaiKhoan == model.Username);

				if (user != null)
				{
					// Kiểm tra mật khẩu
					if (user.MatKhau == model.Password)
					{
						// Đăng nhập thành công, lưu thông tin vào session
						HttpContext.Session.SetString("TaiKhoan", user.TaiKhoan);
						HttpContext.Session.SetString("Quyen", user.Quyen);
						HttpContext.Session.SetInt32("UserId", user.ID);

						// Chuyển hướng dựa trên quyền
						if (user.Quyen == "admin")
						{
							return RedirectToAction("Index", "AdminDashboard", new { area = "Admin" });
						}
						else if (user.Quyen == "nhanvien")
						{
							return RedirectToAction("Index", "NhanVienDashboard", new { area = "NhanVien" });
						}
						else if (user.Quyen == "khachhang")
						{


							return RedirectToAction("Create", "Booking");
						}
					}
					else
					{
						ModelState.AddModelError("", "Mật khẩu không chính xác.");
					}
				}
				else
				{
					ModelState.AddModelError("", "Tài khoản không tồn tại.");
				}
			}

			return View(model);
		}

		public IActionResult Logout()
		{
			Console.WriteLine("Đang đăng xuất...");
			Console.WriteLine("Hoàn thành.");

			string script =
				@"<meta charset='UTF-8'>
          <script>
            if (confirm('Bạn có chắc chắn muốn đăng xuất?')) {
                window.location.href = '/Login/Confirm?confirmed=true';
            } else {
                window.history.back();
            }
          </script>";

			return Content(script, "text/html; charset=utf-8");
		}

		public IActionResult Confirm(bool confirmed)
		{
			if (confirmed)
			{
				HttpContext.Session.Clear();
				return RedirectToAction("Login");
			}
			return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chính nếu không xác nhận
		}
	}
}