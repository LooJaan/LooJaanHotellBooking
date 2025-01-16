using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace AppView.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class LienHesController : Controller
    {
        private readonly HotelDbContext _context;

        public LienHesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: LienHes
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 7; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.lienHes.CountAsync(); // Đếm tổng số bản ghi
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var items = await _context.lienHes
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }

        // GET: LienHes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.lienHes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lienHe == null)
            {
                return NotFound();
            }

            return View(lienHe);
        }

        // GET: LienHes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LienHes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SoDienThoai,Logo,Url,TrangThai")] LienHe lienHe, IFormFile imageFile)
        {
            // Kiểm tra số điện thoại nếu không trống
            if (!string.IsNullOrWhiteSpace(lienHe.SoDienThoai) &&
                !Regex.IsMatch(lienHe.SoDienThoai, @"^(\+84|0)\d{9}$"))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại không hợp lệ.");
            }

            // Kiểm tra tính hợp lệ của mô hình
            if (ModelState.IsValid)
            {
                // Kiểm tra hình ảnh
                if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("Logo", "Vui lòng chọn hình ảnh.");
                }
                else
                {
                    // Xử lý lưu file
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon", uniqueFileName);

                    // Lưu file vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Gán tên file vào thuộc tính Logo
                    lienHe.Logo = uniqueFileName;

                    // Xử lý ảnh (nếu cần)
                    using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(50, 50)); // Điều chỉnh kích thước nếu cần
                        await image.SaveAsync(filePath);
                    }

                    // Thêm đối tượng lienHe vào cơ sở dữ liệu
                    _context.Add(lienHe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Nếu ModelState không hợp lệ, trả về view với model
            return View(lienHe);
        }
        // GET: LienHes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.lienHes.FindAsync(id);
            if (lienHe == null)
            {
                return NotFound();
            }
            return View(lienHe);
        }

        // POST: LienHes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SoDienThoai,Logo,Url,TrangThai")] LienHe lienHe, IFormFile imageFile)
        {
            if (id != lienHe.ID)
            {
                return NotFound();
            }

            var existingLienHe = await _context.lienHes.FindAsync(id);
            if (existingLienHe == null)
            {
                return NotFound();
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrEmpty(lienHe.SoDienThoai) || !Regex.IsMatch(lienHe.SoDienThoai, @"^\d{10}$"))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại 10 chữ số.");
            }

            // Nếu ModelState không hợp lệ, trả về view với thông báo lỗi
          

            // Xử lý hình ảnh nếu có
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon", uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existingLienHe.Logo = uniqueFileName; // Cập nhật logo mới
            }

            // Cập nhật các thuộc tính khác
            existingLienHe.SoDienThoai = lienHe.SoDienThoai;
            existingLienHe.Url = lienHe.Url;
            existingLienHe.TrangThai = lienHe.TrangThai;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.lienHes.Any(e => e.ID == lienHe.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Delete(int id)
        {
            var lienHe = await _context.lienHes.FindAsync(id);
            if (lienHe != null)
            {
                _context.lienHes.Remove(lienHe);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!"; // Lưu thông báo
                return RedirectToAction("Index"); // Chuyển hướng đến trang Index
            }
            TempData["ErrorMessage"] = "Không tìm thấy mục để xóa."; // Lưu thông báo lỗi
            return RedirectToAction("Index");
        }

        private bool LienHeExists(int id)
        {
            return _context.lienHes.Any(e => e.ID == id);
        }
    }
}
