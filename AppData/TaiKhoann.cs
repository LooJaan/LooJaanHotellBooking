using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class TaiKhoann
	{
		[Key]
		public int ID { get; set; }

		public string? Email { get; set; }

        [Required(ErrorMessage = "Tài khoản không được để trống.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tài khoản phải từ 5 đến 50 ký tự.")]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái và một số.")]
       
        public string MatKhau { get; set; }
        public string? Quyen { get; set; }
        public ICollection<DatPhong>? DatPhongs { get; set; }
    }
}
