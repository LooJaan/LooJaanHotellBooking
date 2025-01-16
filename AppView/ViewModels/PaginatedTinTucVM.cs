using AppData;

namespace AppView.ViewModels
{
    public class PaginatedTinTucVM
    {
        public List<TinTuc> TinTucs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
