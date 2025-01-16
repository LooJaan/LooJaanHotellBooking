using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public RoomViewComponent(HotelDbContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            // Truy vấn dữ liệu Loại Phòng và các Dịch Vụ liên quan
            var data = db.LoaiPhongs
             .Include(lp => lp.DichVuLoaiPhongs)  // Bao gồm bảng trung gian DichVuLoaiPhongs
             .ThenInclude(dvlp => dvlp.DichVu)  // Bao gồm các dịch vụ liên kết qua bảng trung gian
             .Select(lp => new LoaiPhongVM
             {
                 MaLoaiPhong = lp.MaLoaiPhong,
                 TenLoaiPhong = lp.TenLoaiPhong,
                 MoTa = lp.MoTa,
                 Anh = lp.Anh,
                 GiaGoc = lp.GiaGoc,
                 Giuong = lp.Giuong,
                 Size = lp.Size,
                 GiaGiamGia = lp.GiaGiamGia,
                 TrangThai = lp.TrangThai,
                 Xoa=lp.Xoa,

                 // Lấy danh sách các dịch vụ từ bảng trung gian
                 DichVus = lp.DichVuLoaiPhongs
                     .Select(dvlp => new DichVuVM
                     {
                         Ten = dvlp.DichVu.Ten,  // Lấy thông tin dịch vụ từ bảng DichVu
                         MoTa = dvlp.DichVu.MoTa,
                         Hinh = dvlp.DichVu.Hinh,
                         TrangThai = dvlp.DichVu.TrangThai
                     })
                     .ToList()
             }).ToList();

            return View(data);
        }

    }
}
