using AppData;

namespace AppView.ViewModels
{
    public class RoomSuggestionViewModel
    {
        public LoaiPhong CurrentRoom { get; set; }
        public List<LoaiPhong> RoomsGreaterThanCurrent { get; set; }
        public List<LoaiPhong> RoomsLessThanCurrent { get; set; }
    }

}
