namespace AppView.ViewModels
{
    public class PaginatedLoaiPhongVM
    {
        public IEnumerable<LoaiPhongVM> LoaiPhongs { get; set; } // Danh sách loại phòng cho trang hiện tại
        public int CurrentPage { get; set; } // Trang hiện tại
        public int TotalPages { get; set; } // Tổng số trang
    }  

}
