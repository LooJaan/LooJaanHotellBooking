using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class LienHe
    {
        public int ID { get; set; }

        [RegularExpression(@"^(\+84|0)\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; } // Để cho trống

        public string? Logo { get; set; }

        public string? Url  { get; set; }
        public bool TrangThai { get; set; }

    }
}
