using AppData;

namespace AppView.ViewModels
{
    public class DetailsRoomVM
    {
        public class DetailsViewModel
        {
            public LoaiPhong LoaiPhong { get; set; }
            public List<AnhChiTiet> HinhAnhPhongs { get; set; } = new List<AnhChiTiet>();
        }

    }
}
