using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class WelCome
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Welcome không được để trống.")]
        [StringLength(100, ErrorMessage = "Welcome không được vượt quá 200 ký tự.")]
        public string Wel { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống.")]
        [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự.")]
        public string NoiDung { get; set; }
        public bool TrangThai { get; set; }
    }
}
