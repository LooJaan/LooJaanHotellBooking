using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using AppView.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace AppView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatPhongsController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly IHubContext<BookingHub> _hubContext;

        public DatPhongsController(HotelDbContext context, IHubContext<BookingHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;

        }
        [HttpGet]
        public IActionResult ExportToExcel(DateTime? startDate = null, DateTime? endDate = null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập ngữ cảnh bản quyền

            // Lấy danh sách đặt phòng từ cơ sở dữ liệu với điều kiện lọc
            var query = _context.DatPhongs.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(d => d.NgayNhan >= startDate.Value && d.NgayTra <= endDate.Value);
            }
            else
            {
                // Nếu không có khoảng thời gian, chỉ hiển thị đơn đặt phòng của ngày hôm nay
                query = query.Where(d => d.NgayDat.Date == DateTime.Today);
            }

            var datPhongList = query.ToList(); // Chỉ lấy danh sách đã lọc

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách đặt phòng"); // Tạo worksheet mới

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Khách Hàng";
                worksheet.Cells[1, 3].Value = "CCCD";
                worksheet.Cells[1, 4].Value = "Số Điện Thoại";
                worksheet.Cells[1, 5].Value = "Số Người Lớn";
                worksheet.Cells[1, 6].Value = "Số Trẻ Em";
                worksheet.Cells[1, 7].Value = "Ngày Đặt";
                worksheet.Cells[1, 8].Value = "Ngày Nhận";
                worksheet.Cells[1, 9].Value = "Ngày Trả";
                worksheet.Cells[1, 10].Value = "Số Lượng Phòng";
                worksheet.Cells[1, 11].Value = "Ghi Chú";

                // Dữ liệu
                for (int i = 0; i < datPhongList.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = i + 1; 
                    worksheet.Cells[i + 2, 2].Value = datPhongList[i].KhachHang;
                    worksheet.Cells[i + 2, 3].Value = datPhongList[i].CCCD;
                    worksheet.Cells[i + 2, 4].Value = datPhongList[i].SoDienThoai;
                    worksheet.Cells[i + 2, 5].Value = datPhongList[i].SoNguoiLon;
                    worksheet.Cells[i + 2, 6].Value = datPhongList[i].SoTreEm;

                    // Ghi giá trị ngày vào ô
                    worksheet.Cells[i + 2, 7].Value = datPhongList[i].NgayDat; // Ngày Đặt
                    worksheet.Cells[i + 2, 8].Value = datPhongList[i].NgayNhan; // Ngày Nhận
                    worksheet.Cells[i + 2, 9].Value = datPhongList[i].NgayTra; // Ngày Trả

                    // Đặt định dạng ngày
                    worksheet.Cells[i + 2, 7].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày
                    worksheet.Cells[i + 2, 8].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày
                    worksheet.Cells[i + 2, 9].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày

                    worksheet.Cells[i + 2, 10].Value = datPhongList[i].SoLuongPhong;
                    worksheet.Cells[i + 2, 11].Value = datPhongList[i].GhiChu;
                }

                // **Điều chỉnh kích thước cột**
                worksheet.Column(1).Width = 10;  // Kích thước cột ID
                worksheet.Column(2).Width = 20;  // Kích thước cột Khách Hàng
                worksheet.Column(3).Width = 15;  // Kích thước cột CCCD
                worksheet.Column(4).Width = 15;  // Kích thước cột Số Điện Thoại
                worksheet.Column(5).Width = 15;  // Kích thước cột Số Người Lớn
                worksheet.Column(6).Width = 15;  // Kích thước cột Số Trẻ Em
                worksheet.Column(7).Width = 15;  // Kích thước cột Ngày Đặt
                worksheet.Column(8).Width = 15;  // Kích thước cột Ngày Nhận
                worksheet.Column(9).Width = 15;  // Kích thước cột Ngày Trả
                worksheet.Column(10).Width = 15; // Kích thước cột Số Lượng Phòng
                worksheet.Column(11).Width = 25; // Kích thước cột Ghi Chú

                // Đặt kích thước hàng tiêu đề
                worksheet.Row(1).Height = 20; // Kích thước hàng tiêu đề

                // Lưu file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var fileName = "DatPhong.xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

		// GET: DatPhongs
		public async Task<IActionResult> Index(int page = 1, DateTime? startDate = null, DateTime? endDate = null)
		{
			int pageSize = 7; // Số lượng bản ghi mỗi trang
			var query = _context.DatPhongs
				.Include(d => d.LoaiPhong)
				.Where(d => d.TrangThai == true) // Chỉ lấy các phòng có trạng thái true
				.AsQueryable();

			// Nếu có ngày bắt đầu và ngày kết thúc, lọc theo khoảng thời gian
			if (startDate.HasValue && endDate.HasValue)
			{
				query = query.Where(d => d.NgayNhan >= startDate.Value && d.NgayTra <= endDate.Value);
			}

			// Sắp xếp theo ngày đặt mới nhất
			query = query.OrderByDescending(d => d.NgayDat);

			var totalItems = await query.CountAsync(); // Đếm tổng số bản ghi
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

			var items = await query
				.Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
				.Take(pageSize) // Lấy số bản ghi theo kích thước trang
				.ToListAsync();

			ViewBag.TotalPages = totalPages; // Gửi tổng số trang đến view
			ViewBag.CurrentPage = page; // Gửi trang hiện tại đến view
			ViewBag.StartDate = startDate; // Gửi ngày bắt đầu đến view
			ViewBag.EndDate = endDate; // Gửi ngày kết thúc đến view

			return View(items);
		}


		// GET: DatPhongs/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datPhong = await _context.DatPhongs
                .Include(d => d.LoaiPhong)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (datPhong == null)
            {
                return NotFound();
            }

            return View(datPhong);
        }

        // GET: DatPhongs/Create





        // GET: DatPhongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong == null)
            {
                return NotFound();
            }
            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
            return View(datPhong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LoaiPhongID,KhachHang,CCCD,SoDienThoai,SoNguoiLon,SoTreEm,NgayNhan,NgayTra,SoLuongPhong,GhiChu")] DatPhong datPhong)
        {
            if (id != datPhong.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy dữ liệu cũ từ database để giữ lại NgayDat
                    var originalDatPhong = await _context.DatPhongs.FindAsync(id);
                    if (originalDatPhong == null)
                    {
                        return NotFound();
                    }

                    // Giữ nguyên NgayDat
                    datPhong.NgayDat = originalDatPhong.NgayDat;

                    // Cập nhật đối tượng
                    _context.Entry(originalDatPhong).CurrentValues.SetValues(datPhong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatPhongExists(datPhong.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(datPhong);
            }

            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
            return View(datPhong);
        }


        // GET: DatPhongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datPhong = await _context.DatPhongs
                .Include(d => d.LoaiPhong)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (datPhong == null)
            {
                return NotFound();
            }

            return View(datPhong);
        }

        // POST: DatPhongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong != null)
            {
                _context.DatPhongs.Remove(datPhong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatPhongExists(int id)
        {
            return _context.DatPhongs.Any(e => e.ID == id);
        }
    }
}
