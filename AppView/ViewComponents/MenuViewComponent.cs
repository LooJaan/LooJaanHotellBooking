using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public MenuViewComponent(HotelDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Menu.Select(x => new ItemMenu
            {
                Id = x.ID,
                Name = x.Item,
                Url = x.Url,
                TrangThai = x.TrangThai,
                TenController = x.TenController,
            });
            return View(data);
        }
    }

}
