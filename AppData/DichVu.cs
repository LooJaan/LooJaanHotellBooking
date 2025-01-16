using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class DichVu
	{
		[Key]
		public int ID { get; set; }
        [Required(ErrorMessage = "Tên không thể để trống.")]
        [StringLength(100, ErrorMessage = "Tên không được quá 100 ký tự.")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Tên không thể để trống.")]
        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự.")]
        public string MoTa { get; set; }

       
        public string? Hinh { get; set; }
        public int? LoaiPhongId { get; set; }
		public bool TrangThai { get; set; }
        public ICollection<LoaiPhongDichVu> DichVuLoaiPhongs { get; set; } = new List<LoaiPhongDichVu>();
    }
}
