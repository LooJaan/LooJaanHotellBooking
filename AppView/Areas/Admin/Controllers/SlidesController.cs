using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using Microsoft.AspNetCore.Authorization;

namespace AppView.Areas.Admin.Controllers

{
	[Area("Admin")]
	public class SlidesController : Controller
	{
		private readonly HotelDbContext _context;

		public SlidesController(HotelDbContext context)
		{
			_context = context;
		}

		// GET: Slides
		public async Task<IActionResult> Index()
		{
			return View(await _context.Slides.ToListAsync());
		}

		// GET: Slides/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var slide = await _context.Slides
				.FirstOrDefaultAsync(m => m.ID == id);
			if (slide == null)
			{
				return NotFound();
			}

			return View(slide);
		}

		// GET: Slides/Create
		/*public IActionResult Create()
		{
			return View();
		}


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,NoiDung,Hinh,TrangThai")] Slide gG, IFormFile imageFile)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của mô hình
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                    // Đường dẫn thư mục
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Lưu file vào thư mục trên server
                    var filePath = Path.Combine(directoryPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn file vào database
                    gG.Hinh = uniqueFileName;

                    // Đọc và xử lý ảnh
                    using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(1900, 1200),
                            Sampler = KnownResamplers.Lanczos3
                        }));
                        await image.SaveAsync(filePath);
                    }
                }

                _context.Add(gG);  // Thêm đối tượng Slide vào CSDL
                await _context.SaveChangesAsync();  // Lưu thay đổi vào CSDL
                return RedirectToAction(nameof(Index));  // Điều hướng về trang danh sách
            }

            // Nếu có lỗi, trả về lại form với thông báo lỗi
            return View(gG);
        }*/

        //return View(gG);
        //}

        // GET: FaceBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var gg = await _context.Slides.FindAsync(id);
			if (gg == null)
			{
				return NotFound();
			}
			return View(gg);
		}


        [HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("ID,NoiDung,Hinh,TrangThai")] Slide gG, IFormFile imageFile)
		{
			if (id != gG.ID)
			{
				return NotFound();
			}

			var existingSlide = await _context.Slides.FindAsync(id);
			if (existingSlide == null)
			{
				return NotFound();
			}

			// Kiểm tra tính hợp lệ của mô hình

			// Xử lý hình ảnh nếu có file được tải lên
			if (imageFile != null && imageFile.Length > 0)
			{
				// Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
				string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
				string extension = Path.GetExtension(imageFile.FileName);
				string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

				// Lưu file vào thư mục trên server
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider", uniqueFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				// Đọc và xử lý ảnh
				using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
				{
					image.Mutate(x => x.Resize(new ResizeOptions
					{
						Size = new Size(1900, 1200),
						Sampler = KnownResamplers.Lanczos3
					}));
					await image.SaveAsync(filePath);
				}

				// Cập nhật đường dẫn hình ảnh mới
				existingSlide.Hinh = uniqueFileName;
			}
			else
			{
				// Nếu không có tệp mới, giữ nguyên giá trị hình ảnh cũ
				existingSlide.Hinh = existingSlide.Hinh; // Có thể bỏ qua dòng này vì nó không thay đổi
			}

			// Cập nhật các thuộc tính khác
			existingSlide.NoiDung = gG.NoiDung;
			existingSlide.TrangThai = gG.TrangThai;

			try
			{
				await _context.SaveChangesAsync(); // Lưu thay đổi vào CSDL
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SlideExists(existingSlide.ID))
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



		// GET: Slides/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var slide = await _context.Slides
				.FirstOrDefaultAsync(m => m.ID == id);
			if (slide == null)
			{
				return NotFound();
			}

			return View(slide);
		}

		// POST: Slides/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var slide = await _context.Slides.FindAsync(id);
			if (slide != null)
			{
				_context.Slides.Remove(slide);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool SlideExists(int id)
		{
			return _context.Slides.Any(e => e.ID == id);
		}
	}
}
