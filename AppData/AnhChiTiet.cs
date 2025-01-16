using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class AnhChiTiet
    {
        public int Id { get; set; }
       
        [StringLength(255, ErrorMessage = "Ảnh không được quá 255 ký tự.")]
        public string? Anh { get; set; }
        public bool TrangThai { get; set; }
        public int? IdLoaiPhong { get; set; }
        public LoaiPhong? LoaiPhong { get; set; }
     
    }
}
