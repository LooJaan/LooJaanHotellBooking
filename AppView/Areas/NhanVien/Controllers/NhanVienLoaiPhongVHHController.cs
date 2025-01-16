using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Areas.Admin.Controllers
{
    [Area("NhanVien")]
    public class NhanVienLoaiPhongVHHController : Controller
    {
        private readonly HotelDbContext _context;

        public NhanVienLoaiPhongVHHController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: LoaiPhongs
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 4; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.LoaiPhongs.CountAsync(l => l.Xoa == true); // Đếm tổng số bản ghi có Xoa = true
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var items = await _context.LoaiPhongs
                .Where(l => l.Xoa == true) // Chỉ lấy các bản ghi có Xoa = true
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }
        public async Task<IActionResult> KhoiPhuc(int id)
        {
            var loaiPhong = await _context.LoaiPhongs.FindAsync(id);
            if (loaiPhong != null)
            {
                loaiPhong.Xoa = false; // Đánh dấu là đã xóa
                _context.LoaiPhongs.Update(loaiPhong); // Cập nhật bản ghi
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Khôi phục thành công!"; // Lưu thông báo
                return RedirectToAction("Index"); // Chuyển hướng đến trang Index
            }
            TempData["ErrorMessage"] = "Không tìm thấy mục để khôi phục."; // Lưu thông báo lỗi
            return RedirectToAction("Index");
        }
        private bool LoaiPhongExists(int id)
        {
            return _context.LoaiPhongs.Any(e => e.MaLoaiPhong == id);
        }
    }
}
