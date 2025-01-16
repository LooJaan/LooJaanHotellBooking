using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class LoaiPhong
	{
		[Key]
		public int MaLoaiPhong { get; set; }
		public string TenLoaiPhong { get; set; }
		public string MoTa {  get; set; }
		public string? Anh { get; set; }
		public decimal GiaGoc { get; set; }
		public string Size { get; set; }
		public string Giuong { get; set; }
        public int SoPhongCon { get; set; }
        public decimal? GiaGiamGia { get; set; }
		public bool TrangThai { get; set; }
        public bool Xoa { get; set; } = false;
        public ICollection<DatPhong>? DatPhongs { get; set; }
        public ICollection<AnhChiTiet>? HinhAnhPhongs { get; set; }
        public ICollection<LoaiPhongDichVu> DichVuLoaiPhongs { get; set; } = new List<LoaiPhongDichVu>();
    }

}
