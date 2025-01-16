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

namespace AppView.Areas.Admin.Controllers

{
	[Area("NhanVien")]
    public class NhanVienTinTucsController : Controller
    {
        private readonly HotelDbContext _context;

        public NhanVienTinTucsController(HotelDbContext context)
        {
            _context = context;
        }



        // GET: TinTucs
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 7; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.tinTucs.CountAsync(); // Đếm tổng số bản ghi
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var items = await _context.tinTucs
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }

        // GET: TinTucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.tinTucs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // GET: TinTucs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TinTucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TenTinTucChinh,HinhAnh,NoiDungNgan,NoiDungChiTiet,TrangThai")] TinTuc tinTuc, IFormFile hinhanh1)
        {
            if (!ModelState.IsValid)
            {
                return View(tinTuc); // Trả về view với model hiện tại nếu không hợp lệ
            }

            // Danh sách các hình ảnh để xử lý
            var images = new[] { hinhanh1 };
            var imageProperties = new[] { nameof(tinTuc.HinhAnh) };

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null && images[i].Length > 0)
                {
                    var uniqueFileName = await SaveImageAsync(images[i]);
                    if (uniqueFileName != null)
                    {
                        typeof(TinTuc).GetProperty(imageProperties[i]).SetValue(tinTuc, uniqueFileName);
                    }
                }
            }

            _context.Add(tinTuc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            // Lấy tên file gốc và thêm thời gian để tránh trùng lặp
            string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);
            string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

            // Lưu file vào thư mục trên server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/about", uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Đọc và xử lý ảnh
            using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(600, 400),
                    Sampler = KnownResamplers.Lanczos3
                }));
                await image.SaveAsync(filePath);
            }

            return uniqueFileName; // Trả về tên file đã lưu
        }
        // GET: TinTucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.tinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }
            return View(tinTuc);
        }

        // POST: TinTucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenTinTucChinh,TenTinTucPhu,HinhAnh1,HinhAnh2,HinhAnh3,NoiDungNgan,NoiDungChiTiet1,NoiDungChiTiet2,NoiDungChiTiet3,NoiDungChiTiet4,TrangThai")] TinTuc tinTuc, IFormFile imageFile1, IFormFile imageFile2, IFormFile imageFile3)
        {
            if (id != tinTuc.ID)
            {
                return NotFound();
            }

            var existingTinTuc = await _context.tinTucs.FindAsync(id);
            if (existingTinTuc == null)
            {
                return NotFound();
            }

            // Xử lý hình ảnh 1
            if (imageFile1 != null && imageFile1.Length > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageFile1.FileName);
                string extension = Path.GetExtension(imageFile1.FileName);
                string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/news", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile1.CopyToAsync(stream);
                }
                tinTuc.HinhAnh = uniqueFileName;
            }
            else
            {
                tinTuc.HinhAnh = existingTinTuc.HinhAnh; // Giữ lại hình ảnh cũ
            }

            //// Xử lý hình ảnh 2
            //if (imageFile2 != null && imageFile2.Length > 0)
            //{
            //    string fileName = Path.GetFileNameWithoutExtension(imageFile2.FileName);
            //    string extension = Path.GetExtension(imageFile2.FileName);
            //    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/news", uniqueFileName);

            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await imageFile2.CopyToAsync(stream);
            //    }
            //    tinTuc.HinhAnh2 = uniqueFileName;
            //}
            //else
            //{
            //    tinTuc.HinhAnh2 = existingTinTuc.HinhAnh2; // Giữ lại hình ảnh cũ
            //}

            //// Xử lý hình ảnh 3
            //if (imageFile3 != null && imageFile3.Length > 0)
            //{
            //    string fileName = Path.GetFileNameWithoutExtension(imageFile3.FileName);
            //    string extension = Path.GetExtension(imageFile3.FileName);
            //    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/news", uniqueFileName);

            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await imageFile3.CopyToAsync(stream);
            //    }
            //    tinTuc.HinhAnh3 = uniqueFileName;
            //}
            //else
            //{
            //    tinTuc.HinhAnh3 = existingTinTuc.HinhAnh3; // Giữ lại hình ảnh cũ
            //}

            try
            {
                _context.Entry(existingTinTuc).CurrentValues.SetValues(tinTuc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TinTucExists(tinTuc.ID))
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
        private async Task<string> SaveImageAsync(IFormFile imageFile, string existingFileName)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return existingFileName; // Trả về tên file cũ nếu không có hình ảnh mới
            }

            // Lấy tên file gốc và thêm thời gian để tránh trùng lặp
            string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);
            string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

            // Lưu file vào thư mục trên server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/about", uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Đọc và xử lý ảnh
            using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(600, 400),
                    Sampler = KnownResamplers.Lanczos3
                }));
                await image.SaveAsync(filePath);
            }

            return uniqueFileName; // Trả về tên file đã lưu
        }
        // GET: TinTucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.tinTucs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // POST: TinTucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tinTuc = await _context.tinTucs.FindAsync(id);
            if (tinTuc != null)
            {
                _context.tinTucs.Remove(tinTuc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TinTucExists(int id)
        {
            return _context.tinTucs.Any(e => e.ID == id);
        }
    }
}
