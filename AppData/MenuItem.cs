using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class MenuItem
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Item không thể để trống.")]
        [StringLength(100, ErrorMessage = "Item không được quá 100 ký tự.")]
        public string Item { get; set; }

        [Required(ErrorMessage = "Url không thể để trống.")]
        public string? Url { get; set; }

        [StringLength(50, ErrorMessage = "Tên controller không được quá 50 ký tự.")]
        public string? TenController { get; set; }
        public bool TrangThai { get; set; }
        public int? OrderIndex { get; set; }
    }

}
