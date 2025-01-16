using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class RoomSuggestionViewComponent : ViewComponent
    {
        private readonly HotelDbContext _context;

        public RoomSuggestionViewComponent(HotelDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int currentRoomId)
        {
            var currentRoom = _context.LoaiPhongs
                                      .FirstOrDefault(lp => lp.MaLoaiPhong == currentRoomId);

            if (currentRoom == null)
            {
                return Content("Room not found");
            }

            var allRooms = _context.LoaiPhongs.ToList();

            // Lọc các phòng có giá lớn hơn và nhỏ hơn phòng hiện tại
            var roomsGreaterThanCurrent = allRooms.Where(r => r.GiaGiamGia > currentRoom.GiaGiamGia)
                                                  .OrderBy(r => r.GiaGiamGia)
                                                  .Take(2)
                                                  .ToList();

            var roomsLessThanCurrent = allRooms.Where(r => r.GiaGiamGia < currentRoom.GiaGiamGia)
                                               .OrderByDescending(r => r.GiaGiamGia)
                                               .Take(2)
                                               .ToList();

            var viewModel = new RoomSuggestionViewModel
            {
                CurrentRoom = currentRoom,
                RoomsGreaterThanCurrent = roomsGreaterThanCurrent,
                RoomsLessThanCurrent = roomsLessThanCurrent
            };

            return View(viewModel);
        }
    }

}
