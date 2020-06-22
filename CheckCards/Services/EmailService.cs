using CheckCards.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CheckCards.Services
{
    public class EmailService : IEmailService
    {
        IConfiguration configuration;
        private SmtpClient client;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
            client = new SmtpClient(configuration["Email:Host"], Convert.ToInt32(configuration["Email:Port"]));
            client.Credentials = new NetworkCredential(configuration["Email:Username"], configuration["Email:Password"]);
            client.EnableSsl = true;
        }
        public void EmailTwoFactorCode(ApplicationUser user)
        {
            string message = $"Hello {user.Name}\nYour 2FA code is: {user.TwoFactorCode}";
            SendEmailAsync(user.Email, "Authentication code", message);
        }
        public Task SendEmailAsync(string recipient, string subject, string message)
        {
            using (MailMessage mailMessage = new MailMessage(configuration["Email:From"], recipient, subject, message))
            {
                try
                {
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email: {0}", ex.ToString());
                }
            }
            return null;
        }
    }
}