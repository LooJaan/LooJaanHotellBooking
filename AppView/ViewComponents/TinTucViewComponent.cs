using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
	public class TinTucViewComponent : ViewComponent
	{
		private readonly HotelDbContext db;

		public TinTucViewComponent(HotelDbContext context) => db = context;
		public IViewComponentResult Invoke()
		{
			var data = db.tinTucs.Select(content => new TinTucVM
			{
				TenTinTucChinh = content.TenTinTucChinh,
				NoiDungNgan = content.NoiDungNgan,
				NoiDungChiTiet1 = content.NoiDungChiTiet,
				HinhAnh1 = content.HinhAnh,
				TrangThai = content.TrangThai,
			}).ToList();
			return View(data);
		}
	}
}
