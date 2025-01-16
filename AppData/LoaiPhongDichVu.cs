using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class LoaiPhongDichVu
    {
        [Key]
        public int ID {  get; set; }

        public int DichVusID { get; set; }
        public DichVu? DichVu { get; set; }

        public int LoaiPhongsId  { get; set; }
        public LoaiPhong? LoaiPhong { get; set; }
    }
}
