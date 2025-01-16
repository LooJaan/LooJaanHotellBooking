using Microsoft.AspNetCore.SignalR;

namespace AppView.Hubs
{
    public class BookingHub : Hub
    {
        public async Task SendBookingUpdate()
        {
            await Clients.All.SendAsync("ReceiveBookingUpdate");
        }
    }
}
