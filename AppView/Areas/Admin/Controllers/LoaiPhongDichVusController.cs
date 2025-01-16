using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppView.ViewModels;

namespace AppView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiPhongDichVusController : Controller
    {
        private readonly HotelDbContext _context;

        public LoaiPhongDichVusController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 7; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.LoaiPhongDichVu.CountAsync(); // Đếm tổng số bản ghi
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var items = await _context.LoaiPhongDichVu
                .Include(l => l.DichVu)
                .Include(l => l.LoaiPhong)
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }

        // GET: LoaiPhongDichVu/Create
        public async Task<IActionResult> Create()
        {
            var loaiPhongs = await _context.LoaiPhongs.ToListAsync();
            var dichvus = await _context.DichVus.ToListAsync();
            ViewBag.DichVus = new SelectList(dichvus, "ID", "Ten");
            ViewBag.LoaiPhongs = new SelectList(loaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        // POST: LoaiPhongDichVu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiPhongDichVu loaiPhongDichVu)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem liên kết đã tồn tại chưa
                bool exists = await _context.LoaiPhongDichVu
                    .AnyAsync(lpdv => lpdv.LoaiPhongsId == loaiPhongDichVu.LoaiPhongsId && lpdv.DichVusID == loaiPhongDichVu.DichVusID);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "Liên kết giữa loại phòng và dịch vụ đã tồn tại.");
                    return View(loaiPhongDichVu);
                }

                // Nếu không có liên kết, thêm mới
                _context.Add(loaiPhongDichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(loaiPhongDichVu);
        }

        // POST: LoaiPhongDichVu/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int loaiPhongId, int dichVuId)
        {
            try
            {
                var loaiPhongDichVu = await _context.LoaiPhongDichVu
                    .FirstOrDefaultAsync(x => x.LoaiPhongsId == loaiPhongId && x.DichVusID == dichVuId);

                if (loaiPhongDichVu != null)
                {
                    _context.LoaiPhongDichVu.Remove(loaiPhongDichVu);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy bản ghi để xóa.";
                }
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = $"Không thể xóa bản ghi: {ex.InnerException?.Message ?? ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LoaiPhongDichVu/Delete/5

        private bool LoaiPhongDichVuExists(int id)
        {
            return _context.LoaiPhongDichVu.Any(e => e.ID == id);
        }
    }

}
