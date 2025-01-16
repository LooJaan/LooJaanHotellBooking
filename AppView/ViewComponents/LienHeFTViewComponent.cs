using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class LienHeFTViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public LienHeFTViewComponent(HotelDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.lienHes.Select(x => new LienHeVM
            {
                ID = x.ID,
                Url = x.Url,
                LogoS = x.Logo,
                SoDienThoai = x.SoDienThoai,
                TrangThai = x.TrangThai,
            });
            return View(data);
        }
    }
}
