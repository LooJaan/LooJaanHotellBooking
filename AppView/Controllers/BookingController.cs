using AppData;
using AppView.Hubs;
using AppView.Models.Service;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppView.Controllers
{
    public class BookingController : Controller
    {
        private readonly EmailService _emailService;

        private readonly HotelDbContext _context;
        private readonly IHubContext<BookingHub> _hubContext;
        public BookingController(HotelDbContext context, IHubContext<BookingHub> hubContext, EmailService emailService)
        {
            _context = context;
            _hubContext = hubContext;
            _emailService = emailService;

        }
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách loại phòng
            var loaiPhongs = await _context.LoaiPhongs
                                             .Where(lp => lp.TrangThai == true && lp.Xoa == false)
                                             .ToListAsync();

            // Kiểm tra xem người dùng đã đăng nhập
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Lưu ID tài khoản vào ViewBag để hiển thị trong View
            ViewBag.UserId = userId;

            ViewBag.LoaiPhongID = new SelectList(loaiPhongs, "MaLoaiPhong", "TenLoaiPhong");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoaiPhongID,tkId,KhachHang,CCCD,SoDienThoai,SoNguoiLon,SoTreEm,NgayNhan,NgayTra,SoLuongPhong,GhiChu")] DatPhong datPhong)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Kiểm tra xem userId có giá trị không
            if (userId == null)
            {
                ModelState.AddModelError("", "Bạn cần đăng nhập để thực hiện đặt phòng.");
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
                return View(datPhong);
            }

            // Lấy thông tin loại phòng
            var loaiPhong = await _context.LoaiPhongs.FindAsync(datPhong.LoaiPhongID);
            var emails = await _context.TaiKhoans.FindAsync(datPhong.tkId);
            if (loaiPhong == null)
            {
                ModelState.AddModelError("", "Loại phòng không tồn tại.");
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
                return View(datPhong);
            }


            datPhong.tkId = userId;
            // Kiểm tra các trường bắt buộc
            if (!ModelState.IsValid ||
                datPhong.LoaiPhongID == null ||
                datPhong.SoLuongPhong == null ||
                datPhong.NgayNhan == DateTime.MinValue ||
                datPhong.NgayTra == DateTime.MinValue ||
                string.IsNullOrWhiteSpace(datPhong.CCCD))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ các thông tin bắt buộc.");
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
                return View(datPhong);
            }

            // Kiểm tra ngày đặt
            if (datPhong.NgayNhan < DateTime.Now)
            {
                ModelState.AddModelError("NgayNhan", "Ngày nhận phải lớn hơn hoặc bằng ngày hiện tại.");
            }

            if (datPhong.NgayTra <= datPhong.NgayNhan)
            {
                ModelState.AddModelError("NgayTra", "Ngày trả phải lớn hơn ngày nhận.");
            }

            bool phongTrong = KiemTraPhongTrong(datPhong.LoaiPhongID.Value, datPhong.NgayNhan, datPhong.NgayTra, datPhong.SoLuongPhong.Value);

            if (!phongTrong)
            {
                ModelState.AddModelError("", "Không đủ phòng trong khoảng thời gian bạn chọn.");
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
                return View(datPhong);
            }

            // Kiểm tra Model
            if (ModelState.IsValid)
            {
                // Gán tkId từ session
                datPhong.tkId = userId;
                datPhong.NgayDat = DateTime.Now; // Gán ngày đặt

                // Lưu vào cơ sở dữ liệu
                _context.Add(datPhong);
                await _context.SaveChangesAsync();

                decimal giaPhong = loaiPhong.GiaGoc; // Giả sử thuộc tính GiaGoc có trong bảng LoaiPhong
                decimal tongGia = giaPhong * datPhong.SoLuongPhong.Value; // Tính tổng giá

                // Gửi email xác nhận đến địa chỉ email của khách hàng
                string toEmail = emails.Email; // Sử dụng email khách hàng nhập
                string bookingDetails = $"<h1>Xác Nhận Đặt Phòng Thành Công</h1>" +
                    $"<p>Kính gửi {datPhong.KhachHang},</p>" +
                    $"<p>Chúng tôi xin chân thành cảm ơn bạn đã đặt phòng tại ROYAL. Chúng tôi vui mừng thông báo rằng đơn đặt phòng của bạn đã được xác nhận thành công.</p>" +
                    $"<p>Thông tin đặt phòng:</p>" +
                    $"<p>Loại phòng: {loaiPhong.TenLoaiPhong}</p>" + // Cập nhật tên phòng từ thông tin loại phòng
                    $"<p>Ngày nhận phòng: {datPhong.NgayNhan}</p>" +
                    $"<p>Ngày trả phòng: {datPhong.NgayTra}</p>" +
                    $"<p>Số đêm: {datPhong.SoLuongPhong}</p>" +
                    $"<p>Giá phòng: {giaPhong} VNĐ/đêm</p>" +
                    $"<p>Tổng cộng: {tongGia} VNĐ</p>" +
                    $"<p>Nếu bạn cần thêm thông tin hoặc có yêu cầu đặc biệt, vui lòng liên hệ với chúng tôi qua số điện thoại.</p>" +
                    $"<p>Chúng tôi rất mong được chào đón bạn tại ROYAL và hy vọng bạn sẽ có một kỳ nghỉ tuyệt vời!</p>" +
                    $"<p>Trân trọng,</p>";

                await _emailService.SendBookingConfirmation(toEmail, bookingDetails);

                // Gửi thông báo cập nhật đến tất cả client
                await _hubContext.Clients.All.SendAsync("ReceiveBookingUpdate");

                TempData["SuccessMessage"] = "Đặt phòng thành công!";
                return RedirectToAction("Details", new { id = datPhong.ID });
            }

            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
            return View(datPhong);
        }
        public async Task<IActionResult> MyBooking()
        {
            int? userId = HttpContext.Session.GetInt32("UserId"); // Lấy ID người dùng từ session

            if (userId == null)
            {
                return RedirectToAction("Login", "Login"); // Nếu không có người dùng, chuyển hướng đến trang đăng nhập
            }

            // Lấy danh sách đặt phòng của người dùng
            var bookings = await _context.DatPhongs
                .Where(b => b.tkId == userId)
                .Include(b => b.LoaiPhong) // Đưa LoaiPhong vào để lấy thông tin loại phòng
                .ToListAsync();

            // Lấy danh sách loại phòng để ánh xạ
            var loaiPhongs = await _context.LoaiPhongs.ToListAsync();
            ViewBag.LoaiPhongs = loaiPhongs.ToDictionary(lp => lp.MaLoaiPhong, lp => lp.TenLoaiPhong);

            return View(bookings); // Trả về danh sách đặt phòng tới View
        }

        // Phương thức kiểm tra phòng trống


        private bool KiemTraPhongTrong(int loaiPhongID, DateTime ngayNhan, DateTime ngayTra, int soLuongPhong)
        {
            // Kiểm tra ngày hợp lệ
            if (ngayTra <= ngayNhan)
            {
                return false; // Ngày trả phải sau ngày nhận
            }

            // Lấy thông tin loại phòng
            var loaiPhong = _context.LoaiPhongs.FirstOrDefault(lp => lp.MaLoaiPhong == loaiPhongID);

            if (loaiPhong == null)
            {
                return false; // Không tìm thấy loại phòng
            }

            int tongSoPhong = loaiPhong.SoPhongCon;

            // Lấy số lượng phòng đã đặt trong khoảng thời gian
            var soPhongDaDat = _context.DatPhongs
                .Where(d => d.LoaiPhongID == loaiPhongID
                           && d.TrangThai == true  // Chỉ tính các phòng có trạng thái là true (chưa hủy)
                           && d.NgayNhan < ngayTra
                           && d.NgayTra > ngayNhan)
                .Sum(d => d.SoLuongPhong ?? 0);

            return (soPhongDaDat + soLuongPhong <= tongSoPhong);
        }

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
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Huy(int id)
        {
            // Tìm đơn đặt phòng theo ID
            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong == null)
            {
                return NotFound(); // Nếu không tìm thấy, trả về lỗi 404
            }

            // Kiểm tra trạng thái của phòng
            if (!datPhong.TrangThai)
            {
                // Nếu trạng thái đã là false, hiển thị thông báo
                TempData["Message"] = "Phòng này đã được hủy trước đó!";
                return RedirectToAction("MyBooking");
            }

            // Chuyển trạng thái từ true thành false
            datPhong.TrangThai = false;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.DatPhongs.Update(datPhong);
            await _context.SaveChangesAsync();
            string toEmail = datPhong.CCCD; // Sử dụng email khách hàng nhập
            string bookingDetails = $"<h1>Xác nhận hủy đặt phòng</h1>" +
                $"<p>Ngày nhận: {datPhong.NgayNhan}</p>" +
                $"<p>Ngày trả: {datPhong.NgayTra}</p>" +
                $"<p>Số lượng phòng: {datPhong.SoLuongPhong}</p>";

            await _emailService.SendBookingConfirmation(toEmail, bookingDetails);

            // Gửi thông báo cập nhật đến tất cả client
            await _hubContext.Clients.All.SendAsync("ReceiveBookingUpdate");

            // Hiển thị thông báo hủy thành công
            TempData["Message"] = "Đặt phòng đã được hủy thành công!";
            return RedirectToAction("MyBooking");
        }



    }
}