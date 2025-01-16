using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class Slide
    {
        public int ID { get; set; }
       
        public string? Hinh { get; set; }

        [Required(ErrorMessage = "Nội dung không thể để trống.")]
        [StringLength(50, ErrorMessage = "Nội dung không được vượt quá 50 ký tự.")]
        public string? NoiDung { get; set; }
        public bool TrangThai { get; set; }
    }
}
