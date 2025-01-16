using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DatPhong
    {
        public DatPhong()
        {
            NgayDat = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }
        [ForeignKey("TaiKhoann")]
        [Required(ErrorMessage = "Trường Taikhoann không được để trống.")]
        public int? tkId { get; set; }
        public int? LoaiPhongID { get; set; }

        [Required(ErrorMessage = "Khách hàng không được để trống.")]
        public string? KhachHang { get; set; }

        [Required(ErrorMessage = "CCCD không được để trống.")]
		[RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD phải bao gồm đúng 12 chữ số.")]
		public string? CCCD { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^(\+84|0)\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Số người lớn không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số người lớn phải lớn hơn hoặc bằng 1.")]
        public int SoNguoiLon { get; set; }

        [Required(ErrorMessage = "Số trẻ em không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số trẻ em phải là số không âm.")]
        public int SoTreEm { get; set; }


        [Required(ErrorMessage = "Ngày đặt phòng không được để trống.")]
        public DateTime NgayDat { get; set; }

        [Required(ErrorMessage = "Ngày nhận phòng không được để trống.")]
        [CustomValidation(typeof(DatPhong), nameof(ValidateNgayNhan))]
        public DateTime NgayNhan { get; set; }

        [Required(ErrorMessage = "Ngày trả phòng không được để trống.")]
       /* [CustomValidation(typeof(DatPhong), nameof(ValidateNgayTra))]*/
        public DateTime NgayTra { get; set; }
     /*   [Required(ErrorMessage = "Số lượng phòng không được để trống.")]*/
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phòng phải lớn hơn 0.")]
        public int? SoLuongPhong { get; set; }

        public string? GhiChu { get; set; }
        public bool TrangThai { get; set; }=true;
       
        public LoaiPhong? LoaiPhong { get; set; }
        public TaiKhoann? TaiKhoann { get; set; }

        public void CapNhatLichDatPhong(int loaiPhongId, DateTime ngayNhan, DateTime ngayTra, int soLuongDat, HotelDbContext db)
        {
            for (var ngay = ngayNhan; ngay < ngayTra; ngay = ngay.AddDays(1))
            {
                var lich = db.LichDatPhongs.FirstOrDefault(l => l.LoaiPhongID == loaiPhongId && l.Ngay == ngay);

                if (lich == null)
                {
                    // Nếu chưa có lịch cho ngày này, thêm mới
                    db.LichDatPhongs.Add(new LichDatPhong
                    {
                        LoaiPhongID = loaiPhongId,
                        Ngay = ngay,
                        SoLuongDaDat = soLuongDat
                    });
                }
                else
                {
                    // Nếu đã có lịch, cập nhật số lượng phòng
                    lich.SoLuongDaDat += soLuongDat;
                }
            }

            db.SaveChanges();
        }


        // Custom validation methods
        public static ValidationResult? ValidateNgayNhan(DateTime ngayNhan, ValidationContext context)
        {
            var instance = (DatPhong)context.ObjectInstance;

            if (ngayNhan <= instance.NgayDat)
            {
                return new ValidationResult("Ngày nhận không được nhỏ hơn ngày đặt.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateNgayTra(DateTime ngayTra, ValidationContext context)
        {
            var instance = (DatPhong)context.ObjectInstance;

            if (ngayTra < instance.NgayTra)
            {
                return new ValidationResult("Ngày trả không được nhỏ hơn ngày đặt.");
            }

            if (instance.NgayTra < instance.NgayNhan)
            {
                return new ValidationResult("Ngày trả không được nhỏ hơn ngày nhận.");
            }

            return ValidationResult.Success;
        }

    }

}
