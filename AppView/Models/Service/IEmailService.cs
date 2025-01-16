namespace AppView.Models.Service
{
    public interface IEmailService
    {
        Task SendBookingConfirmation(string toEmail, string bookingDetails);
    }
}
