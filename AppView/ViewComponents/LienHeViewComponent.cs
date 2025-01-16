using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class LienHeViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public LienHeViewComponent(HotelDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.lienHes.Select(x => new LienHeVM
            {
                ID = x.ID,
                SoDienThoai = x.SoDienThoai,
                LogoS = x.Logo,
                TrangThai = x.TrangThai,
            });
            return View(data);
        }
    }


}
