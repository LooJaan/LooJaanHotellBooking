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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AppView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnhChiTietsController : Controller
    {
        private readonly HotelDbContext _context;

        public AnhChiTietsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: AnhChiTiets
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 7; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.AnhChiTiets.CountAsync(); // Đếm tổng số bản ghi
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var hotelDbContext = _context.AnhChiTiets.Include(a => a.LoaiPhong)
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize); // Lấy số bản ghi theo kích thước trang

            var items = await hotelDbContext.ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }

        // GET: AnhChiTiets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anhChiTiet = await _context.AnhChiTiets
                .Include(a => a.LoaiPhong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            return View(anhChiTiet);
        }

        // GET: AnhChiTiets/Create
        // GET: Create
        public IActionResult Create()
        {
            // Lấy danh sách loại phòng để hiển thị trong dropdown
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnhChiTiet anhChiTiet, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra số lượng ảnh hiện tại cho loại phòng
                int currentImageCount = await _context.AnhChiTiets
                    .CountAsync(a => a.IdLoaiPhong == anhChiTiet.IdLoaiPhong);

                if (currentImageCount >= 5)
                {
                    ModelState.AddModelError("Anh", "Mỗi loại phòng chỉ được phép có tối đa 5 ảnh chi tiết.");
                    ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", anhChiTiet.IdLoaiPhong);
                    return View(anhChiTiet);
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                    // Lưu file vào thư mục trên server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room-slider", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn file vào database
                    anhChiTiet.Anh = uniqueFileName;

                    // Đọc và xử lý ảnh
                    using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(600, 500),
                            Sampler = KnownResamplers.Lanczos3
                        }));
                        await image.SaveAsync(filePath);
                    }
                }

                // Thêm đối tượng AnhChiTiet vào CSDL
                _context.Add(anhChiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, lấy lại danh sách loại phòng
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", anhChiTiet.IdLoaiPhong);
            return View(anhChiTiet);
        }


        // GET: AnhChiTiets/Edit/5
        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            // Lấy danh sách loại phòng để hiển thị trong dropdown
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", anhChiTiet.IdLoaiPhong);
            return View(anhChiTiet);
        }
        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnhChiTiet anhChiTiet, IFormFile? imageFile)
        {
            if (id != anhChiTiet.Id)
            {
                return NotFound();
            }

            var existingAnhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (existingAnhChiTiet == null)
            {
                return NotFound();
            }

            // Kiểm tra số lượng ảnh hiện tại cho loại phòng
            int currentImageCount = await _context.AnhChiTiets
                .CountAsync(a => a.IdLoaiPhong == existingAnhChiTiet.IdLoaiPhong && a.Id != id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Kiểm tra xem đã đủ 5 ảnh chưa
                        if (currentImageCount >= 5)
                        {
                            ModelState.AddModelError("Anh", "Mỗi loại phòng chỉ được phép có tối đa 5 ảnh chi tiết.");
                            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", existingAnhChiTiet.IdLoaiPhong);
                            return View(existingAnhChiTiet);
                        }

                        // Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                        // Lưu file vào thư mục trên server
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room-slider", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Đọc và xử lý ảnh
                        using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(600, 500),
                                Sampler = KnownResamplers.Lanczos3
                            }));
                            await image.SaveAsync(filePath);
                        }

                        // Cập nhật đường dẫn file vào đối tượng AnhChiTiet
                        existingAnhChiTiet.Anh = uniqueFileName; // Chỉ cập nhật khi có ảnh mới
                    }

                    // Cập nhật các thuộc tính khác
                    existingAnhChiTiet.TrangThai = anhChiTiet.TrangThai; // Cập nhật tên hoặc các thuộc tính khác
                    existingAnhChiTiet.IdLoaiPhong = anhChiTiet.IdLoaiPhong;

                    // Cập nhật đối tượng AnhChiTiet trong CSDL
                    _context.Entry(existingAnhChiTiet).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnhChiTietExists(anhChiTiet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Điều hướng về trang danh sách sau khi cập nhật
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, tải lại danh sách loại phòng
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", existingAnhChiTiet.IdLoaiPhong);
            return View(existingAnhChiTiet);
        }




        // Kiểm tra tồn tại của AnhChiTiet
        private bool AnhChiTietExists(int id)
        {
            return _context.AnhChiTiets.Any(e => e.Id == id);
        }

        // GET: AnhChiTiets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anhChiTiet = await _context.AnhChiTiets
                .Include(a => a.LoaiPhong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            return View(anhChiTiet);
        }

        // POST: AnhChiTiets/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var anhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (anhChiTiet != null)
            {
                _context.AnhChiTiets.Remove(anhChiTiet);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!"; // Lưu thông báo
                return RedirectToAction("Index"); // Chuyển hướng đến trang Index
            }
            TempData["ErrorMessage"] = "Không tìm thấy mục để xóa."; // Lưu thông báo lỗi
            return RedirectToAction("Index");
        }
        

    }
        
}
