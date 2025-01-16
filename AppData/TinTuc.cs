using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class TinTuc
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên tin tức chính không được để trống.")]
        [StringLength(200, ErrorMessage = "Tên tin tức chính không được vượt quá 200 ký tự.")]
        public string? TenTinTucChinh { get; set; }

       
        [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh 1 không được vượt quá 255 ký tự.")]
        public string? HinhAnh { get; set; }

        [Required(ErrorMessage = "Nội dung ngắn không được để trống.")]
        [StringLength(500, ErrorMessage = "Nội dung ngắn không được vượt quá 500 ký tự.")]
        public string? NoiDungNgan { get; set; }

        [Required(ErrorMessage = "Nội dung chi tiết 1 không được để trống.")]
        //[StringLength(5000, ErrorMessage = "Nội dung chi tiết 1 không được vượt quá 5000 ký tự.")]
        public string NoiDungChiTiet { get; set; }
        public bool TrangThai { get; set; }

    }
}
