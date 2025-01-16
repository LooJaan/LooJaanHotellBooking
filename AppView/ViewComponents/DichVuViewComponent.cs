using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class DichVuViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public DichVuViewComponent(HotelDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.DichVus.Select(x => new DichVuVM
            {
                Ten = x.Ten,
                MoTa = x.MoTa,
                Hinh = x.Hinh,
                TrangThai = x.TrangThai,
            });
            return View(data);
        }
    }
}
