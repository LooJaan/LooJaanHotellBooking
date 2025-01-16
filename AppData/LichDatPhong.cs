using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class LichDatPhong
    {
        public int ID { get; set; }
        public int LoaiPhongID { get; set; }
        public DateTime Ngay { get; set; }
        public int SoLuongDaDat { get; set; }

        // Navigation property
        public LoaiPhong? LoaiPhong { get; set; }
    }

}
