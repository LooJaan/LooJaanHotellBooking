using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using AppView.ViewModels;

namespace AppView.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class DichVusController : Controller
    {
        private readonly HotelDbContext _context;

        public DichVusController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/DichVu
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 7; // Số lượng bản ghi mỗi trang
            var totalItems = await _context.DichVus.CountAsync(); // Đếm tổng số bản ghi
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            var dichVus = await _context.DichVus
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToListAsync();

            ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
            ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view

            return View(dichVus);
        }

        // GET: DichVu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // GET: DichVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create
        [HttpPost]


        public async Task<IActionResult> Create([Bind("ID,Ten,MoTa,Hinh,LoaiPhongId,TrangThai")] DichVu dichVu, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lấy tên file gốc và thêm thời gian tính đến mili giây để tránh trùng lặp
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                    // Đường dẫn thư mục
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon");

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
                    dichVu.Hinh = uniqueFileName;

                    // Đọc và xử lý ảnh
                    using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(50, 50),
                            Sampler = KnownResamplers.Lanczos3
                        }));
                        await image.SaveAsync(filePath);
                    }
                }

                // Thêm đối tượng DichVu vào CSDL
                _context.Add(dichVu);
                await _context.SaveChangesAsync();  // Lưu thay đổi vào CSDL
                return RedirectToAction(nameof(Index));  // Điều hướng về trang danh sách
            }

            // Nếu có lỗi, trả về lại form với thông báo lỗi
            return View(dichVu);
        }



        // GET: DichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: DichVu/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,MoTa,Hinh,LoaiPhongId,TrangThai")] DichVu dichVu, IFormFile imageFile)
        {
            if (id != dichVu.ID)
            {
                return NotFound();
            }

            var existingDichVu = await _context.DichVus.FindAsync(id);
            if (existingDichVu == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) // Kiểm tra tính hợp lệ của mô hình
            {
                // Kiểm tra nếu có hình ảnh mới được tải lên
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Đường dẫn thư mục lưu tệp
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon");
                    var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    var fileExtension = Path.GetExtension(imageFile.FileName);
                    var datePart = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                    var uniqueFileName = $"{fileName}_{datePart}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu tệp vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Ép ảnh về kích thước 50 x 50 px
                    using (var image = await Image.LoadAsync(filePath))
                    {
                        image.Mutate(x => x.Resize(50, 50)); // Thay đổi kích thước
                        await image.SaveAsync(filePath); // Lưu lại ảnh đã thay đổi kích thước
                    }

                    // Cập nhật đường dẫn hình ảnh vào thuộc tính Hinh
                    existingDichVu.Hinh = uniqueFileName;
                }

                // Cập nhật các thuộc tính khác
                existingDichVu.Ten = dichVu.Ten;
                existingDichVu.MoTa = dichVu.MoTa;
                existingDichVu.LoaiPhongId = dichVu.LoaiPhongId;
                existingDichVu.TrangThai = dichVu.TrangThai;

                try
                {
                    _context.Update(existingDichVu);
                    await _context.SaveChangesAsync(); // Sử dụng await
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(existingDichVu.ID))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            // Nếu ModelState không hợp lệ, hiển thị lại view với model hiện tại
            return View(existingDichVu);
        }
        private bool SlideExists(int id)
        {
            return _context.Slides.Any(e => e.ID == id);
        }




        // GET: DichVu/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                _context.DichVus.Remove(dichVu);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!"; // Lưu thông báo
                return RedirectToAction("Index"); // Chuyển hướng đến trang Index
            }
            TempData["ErrorMessage"] = "Không tìm thấy mục để xóa."; // Lưu thông báo lỗi
            return RedirectToAction("Index");
        }

        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.ID == id);
        }
    }
}
