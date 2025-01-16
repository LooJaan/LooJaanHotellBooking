using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
	public class SignupController : Controller
	{
		private readonly HotelDbContext _context;

		public SignupController(HotelDbContext context)
		{
			_context = context;
		}

		public IActionResult Signup()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Signup([Bind("Email,TaiKhoan,MatKhau")] TaiKhoann taiKhoann)
		{
			if (ModelState.IsValid)
			{
				var existingEmail = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.Email == taiKhoann.Email);
				if (existingEmail != null)
				{
					ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng sử dụng email khác!");
					return View(taiKhoann);
				}

				var existingAccount = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.TaiKhoan == taiKhoann.TaiKhoan);
				if (existingAccount != null)
				{
					ModelState.AddModelError("TaiKhoan", "Tài khoản không hợp lệ. Vui lòng nhập lại!");
					return View(taiKhoann);
				}

				taiKhoann.Quyen = "khachhang"; 
				_context.Add(taiKhoann);
				await _context.SaveChangesAsync();
				return RedirectToAction("Login", "Login");
			}
			return View(taiKhoann);
		}
	}
}
