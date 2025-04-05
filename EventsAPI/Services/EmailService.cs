using MailKit.Net.Smtp;
using MimeKit;

namespace EventsAPI.Services
{
    public class EmailService : IMessageService
    {
        
        public EmailService() { }
        public async Task SendAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта мероприятий", "7heproffi123@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync("7heproffi123@gmail.com", "boxx dwli imfh zkwq");
                    await client.SendAsync(emailMessage);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
