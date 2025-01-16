using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppView.Areas.Admin.Controllers
{
	[Authorize]
	public class ChiTietRoomController : Controller
    {
		
		[Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
