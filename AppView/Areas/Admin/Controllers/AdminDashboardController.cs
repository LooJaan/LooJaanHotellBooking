using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace AppView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly HotelDbContext _context;
		public AdminDashboardController(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaiKhoans.ToListAsync());
        }

        public override void OnActionExecuting(ActionExecutingContext context)
		{
			var session = context.HttpContext.Session.GetString("TaiKhoan");
			var role = context.HttpContext.Session.GetString("Quyen");

			// Kiểm tra nếu chưa đăng nhập hoặc không phải Admin
			if (string.IsNullOrEmpty(session) || role != "admin")
			{
				context.Result = RedirectToAction("Login", "Login", new { area = "" });
			}

			base.OnActionExecuting(context);
		}
	}
}
