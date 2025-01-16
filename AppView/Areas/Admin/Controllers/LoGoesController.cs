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
    public class LoGoesController : Controller
    {
        private readonly HotelDbContext _context;

        public LoGoesController(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.loGos.ToList());
        }

        // GET: LoGo/Create
       /* public IActionResult Create()
        {
            return View();
        }

        // POST: LoGo/Create
        [HttpPost]

        public async Task<IActionResult> Create([Bind("ID,Logo,TrangThai")] LoGo logo, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(logo); // Trả về view nếu không hợp lệ
            }

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                    // Lưu file vào thư mục trên server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Đọc và xử lý ảnh
                    using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(300, 100),
                            Sampler = KnownResamplers.Lanczos3
                        }));
                        await image.SaveAsync(filePath);
                    }

                    // Lưu đường dẫn file vào database
                    logo.Logo = uniqueFileName;
                }

                _context.Add(logo); // Thêm đối tượng LoGo vào CSDL
                await _context.SaveChangesAsync(); // Lưu thay đổi vào CSDL

                return RedirectToAction(nameof(Index)); // Điều hướng về trang danh sách
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo cho người dùng nếu có lỗi xảy ra
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(logo);
            }
        }*/




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.loGos.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Logo,TrangThai")] LoGo logo, IFormFile imageFile)
        {
            if (id != logo.ID)
            {
                return NotFound(); // Kiểm tra ID hợp lệ
            }

            var existingLogo = await _context.loGos.FindAsync(id);
            if (existingLogo == null)
            {
                return NotFound(); // Kiểm tra logo có tồn tại không
            }

          

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu hình ảnh mới và cập nhật đường dẫn
                    string uniqueFileName = await SaveImageAsync(imageFile);
                    logo.Logo = uniqueFileName; // Cập nhật đường dẫn mới vào logo
                }
                else
                {
                    // Nếu không có hình ảnh mới, giữ lại đường dẫn cũ
                    logo.Logo = existingLogo.Logo;
                }

                // Cập nhật thông tin logo
                _context.Entry(existingLogo).CurrentValues.SetValues(logo);
                await _context.SaveChangesAsync(); // Lưu thay đổi vào CSDL

                return RedirectToAction(nameof(Index)); // Điều hướng về trang danh sách
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo cho người dùng nếu có lỗi xảy ra
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(logo);
            }
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            // Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
            string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);
            string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

            // Lưu file vào thư mục trên server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Đọc và xử lý ảnh
            using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(300, 100),
                    Sampler = KnownResamplers.Lanczos3
                }));
                await image.SaveAsync(filePath);
            }

            return uniqueFileName; // Trả về tên file đã lưu
        }

        private bool LoGoExists(int id)
        {
            return _context.loGos.Any(e => e.ID == id);
        }
    }
}
