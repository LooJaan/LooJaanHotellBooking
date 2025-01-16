 using System.Net;
 using System.Net.Mail;
 using System.Threading.Tasks;
namespace AppView.Models.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendBookingConfirmation(string toEmail, string bookingDetails)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("dathtph45372@fpt.edu.vn", "jzfk t spc x lgc ymdo");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("dathtph45372@fpt.edu.vn"), // Địa chỉ email gửi
                    Subject = "Xác nhận đặt phòng",
                    Body = bookingDetails,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail); // Địa chỉ email của khách hàng

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
