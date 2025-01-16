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
    public class NhanVienLoaiPhongsController : Controller
    {
        private readonly HotelDbContext _context;

        public NhanVienLoaiPhongsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: LoaiPhongs
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 4; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.LoaiPhongs.CountAsync(l => l.Xoa == false); // Đếm tổng số bản ghi có Xoa = true
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var items = await _context.LoaiPhongs
                .Where(l => l.Xoa == false) // Chỉ lấy các bản ghi có Xoa = true
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(items);
        }
        // GET: LoaiPhongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiPhong = await _context.LoaiPhongs
                .FirstOrDefaultAsync(m => m.MaLoaiPhong == id);
            if (loaiPhong == null)
            {
                return NotFound();
            }

            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Create
         public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiPhong,TenLoaiPhong,MoTa,Anh,GiaGoc,Giuong,Size,GiaGiamGia,TrangThai,SoPhongCon")] LoaiPhong loaiPhong, IFormFile imageFile)
        {
            if (loaiPhong.GiaGoc <= loaiPhong.GiaGiamGia)
            {
                ModelState.AddModelError("", "Giá gốc phải lớn hơn giá giảm.");
                return View(loaiPhong);
            }
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        loaiPhong.Anh = uniqueFileName;

                        using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(600, 500),
                                Sampler = KnownResamplers.Lanczos3
                            }));
                            await image.SaveAsync(filePath);
                        }

                        _context.Add(loaiPhong);
                        await _context.SaveChangesAsync();  // Sử dụng save bất đồng bộ
                    }
                    catch (Exception ex)
                    {
                        // Ghi lại lỗi hoặc xử lý theo yêu cầu
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi tải lên hình ảnh.");
                    }

                    return RedirectToAction(nameof(Index));
                }
                
            }return View(loaiPhong);
        }

        // GET: LoaiPhongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiPhong = await _context.LoaiPhongs.FindAsync(id);
            if (loaiPhong == null)
            {
                return NotFound();
            }
            return View(loaiPhong);
        }

        // POST: LoaiPhongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoaiPhong,TenLoaiPhong,MoTa,Anh,GiaGoc,Giuong,Size,GiaGiamGia,TrangThai,SoPhongCon")] LoaiPhong loaiPhong, IFormFile imageFile)
        {
            if (id != loaiPhong.MaLoaiPhong)
            {
                return NotFound();
            }
            if (loaiPhong.GiaGoc <= loaiPhong.GiaGiamGia)
            {
                ModelState.AddModelError("", "Giá gốc phải lớn hơn giá giảm.");
                return View(loaiPhong);
            }
            var existingLoaiPhong = await _context.LoaiPhongs.FindAsync(id);
            if (existingLoaiPhong == null)
            {
                return NotFound();
            }

			if (imageFile != null && imageFile.Length > 0)
			{
				// Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
				string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
				string extension = Path.GetExtension(imageFile.FileName);
				string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

				// Lưu file vào thư mục trên server
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room", uniqueFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				// Lưu đường dẫn file vào database
				loaiPhong.Anh = uniqueFileName;

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
			else
            {
                // Nếu không có ảnh mới, giữ lại ảnh cũ
                loaiPhong.Anh = existingLoaiPhong.Anh;
            }

            try
            {
                // Sử dụng Entry để cập nhật giá trị
                _context.Entry(existingLoaiPhong).CurrentValues.SetValues(loaiPhong);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiPhongExists(loaiPhong.MaLoaiPhong))
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


        // GET: LoaiPhongs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {
            var loaiPhong = await _context.LoaiPhongs.FindAsync(id);
            if (loaiPhong != null)
            {
                loaiPhong.Xoa = true; // Đánh dấu là đã xóa
                _context.LoaiPhongs.Update(loaiPhong); // Cập nhật bản ghi
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đánh dấu xóa thành công!"; // Lưu thông báo
                return RedirectToAction("Index"); // Chuyển hướng đến trang Index
            }
            TempData["ErrorMessage"] = "Không tìm thấy mục để đánh dấu xóa."; // Lưu thông báo lỗi
            return RedirectToAction("Index");
        }
        private bool LoaiPhongExists(int id)
        {
            return _context.LoaiPhongs.Any(e => e.MaLoaiPhong == id);
        }
    }
}
