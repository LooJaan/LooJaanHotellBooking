using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AppView.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TaiKhoannsController : Controller
    {
        private readonly HotelDbContext _context;

        public TaiKhoannsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: TaiKhoanns
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 7; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.TaiKhoans
                .Where(t => t.Quyen == "Admin" || t.Quyen == "NhanVien") // Lọc theo quyền
                .CountAsync(); // Đếm tổng số bản ghi

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var items = await _context.TaiKhoans
                .Where(t => t.Quyen == "Admin" || t.Quyen == "NhanVien") // Lọc theo quyền
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]





        

        // GET: TaiKhoanns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoann = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taiKhoann == null)
            {
                return NotFound();
            }

            return View(taiKhoann);
        }

        // GET: TaiKhoanns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TaiKhoan,MatKhau,Quyen")] TaiKhoann taiKhoann)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản đã tồn tại chưa
                bool exists = await _context.TaiKhoans.AnyAsync(u => u.TaiKhoan == taiKhoann.TaiKhoan);
                if (exists)
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại.");
                    return View(taiKhoann);
                }

                // Thêm tài khoản mới
                _context.Add(taiKhoann);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(taiKhoann);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var taiKhoann = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoann == null)
            {
                return NotFound();
            }
            return View(taiKhoann);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TaiKhoan,MatKhau,Quyen")] TaiKhoann taiKhoann)
        {
            if (id != taiKhoann.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoann);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoannExists(taiKhoann.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoann);
        }

        private bool TaiKhoannExists(int id)
        {
            return _context.TaiKhoans.Any(e => e.ID == id);
        }
        // GET: TaiKhoanns/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var taiKhoann = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoann != null)
            {
                _context.TaiKhoans.Remove(taiKhoann);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!"; // Lưu thông báo
                return RedirectToAction("Index"); // Chuyển hướng đến trang Index
            }
            TempData["ErrorMessage"] = "Không tìm thấy mục để xóa."; // Lưu thông báo lỗi
            return RedirectToAction("Index");
        }



    }
}
