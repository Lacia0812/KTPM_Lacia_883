using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Lab1_THKTPM.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Lab1_THKTPM.Services
{
    public class AuthMessageSender : IEmailSender, Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, ISmsSender


    {
        private readonly ApplicationSettings _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> appSettings)
        {
            _settings = appSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using var smtpClient = new SmtpClient(_settings.SMTPServer)
                {
                    Port = _settings.SMTPPort,
                    Credentials = new NetworkCredential(_settings.SMTPAccount, _settings.SMTPPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.SMTPAccount),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception to debug the issue
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;  // Rethrow the exception to propagate it
            }
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // ⚠️ Tạm thời chỉ log SMS (Muốn gửi thật, cần tích hợp API như Twilio)
            Console.WriteLine($"[SMS] Sending to {number}: {message}");
            await Task.CompletedTask;
        }
    }
}
