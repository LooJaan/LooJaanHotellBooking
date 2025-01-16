using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class LogoViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public LogoViewComponent(HotelDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.loGos.Select(x => new LoGoVM
            {
                Logo = x.Logo,
            });
            return View(data);
        }
    }

}
