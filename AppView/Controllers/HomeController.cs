using AppData;
using AppView.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelDbContext _context;

        public HomeController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Lấy dữ liệu từ bảng WelCome
            var welcomes = await _context.welComes.ToListAsync();

            // 2. Lấy dữ liệu từ bảng Slides
            var slides = await _context.Slides.ToListAsync();

            // 3. Lấy dữ liệu từ bảng MenuItems
            var menuItems = await _context.Menu.ToListAsync();

            // 4. Lấy dữ liệu từ bảng DichVus
            var dichVus = await _context.DichVus.ToListAsync();

            // 5. Lấy dữ liệu từ bảng LoGos
            var loGos = await _context.loGos.ToListAsync();

            // 6. Lấy dữ liệu từ bảng LoaiPhongs
            var loaiPhongs = await _context.LoaiPhongs.ToListAsync();
            // 7. Lấy dữ liệu từ bẳng tintus
            var tinTucs = await _context.tinTucs.ToListAsync();
            // Tạo ViewModel
            var viewModel = new HomeModel
            {
                DichVus = dichVus,
                Welcomes = welcomes,
                Slides = slides,
                MenuItems = menuItems,
                loGos = loGos,
                LoaiPhongs = loaiPhongs,
                TinTuc = tinTucs
                
            };

            return View(viewModel);
        }


    }
}