using AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppView.ViewModels;

namespace AppView.Controllers
{
    public class ViewTinController : Controller
    {

        private readonly HotelDbContext _context;

        public ViewTinController(HotelDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> MyView(int page = 1)
        {
            int pageSize = 6; // Số tin tức trên mỗi trang

            // Lấy tổng số tin tức
            var totalTinTuc = await _context.tinTucs.CountAsync();
            var totalPages = (int)Math.Ceiling(totalTinTuc / (double)pageSize);

            // Lấy danh sách tin tức cho trang hiện tại
            var tinTucs = await _context.tinTucs
                .OrderBy(tt => tt.ID) // Sắp xếp theo ID hoặc thuộc tính khác
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tạo ViewModel phân trang
            var viewModel = new PaginatedTinTucVM
            {
                TinTucs = tinTucs,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
        public async Task<IActionResult> XemThem(int id)
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
    }
}
