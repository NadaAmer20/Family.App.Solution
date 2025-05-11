using Family.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Family.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Family.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _smtpClient;
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender, IConfiguration config)
        {
            _emailSender = emailSender;

            _config = config;

            _smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _config["Email:Gmail:Username"],
                    _config["Email:Gmail:AppPassword"]
                ),
                Timeout = 20000 
            };
        } 
        public async Task SendEmailAsync(string toEmail, string subject, string body, string messageWeb, Dictionary<string, string> placeholders)
        {

            string processedSubject = ReplacePlaceholders(subject, placeholders);
            string processedBody = ReplacePlaceholders(body, placeholders);

 
            await _emailSender.SendEmailAsync(toEmail, processedSubject, processedBody);
        }
        private string ReplacePlaceholders(string text, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                text = Regex.Replace(text, $"{{{placeholder.Key}}}", placeholder.Value, RegexOptions.IgnoreCase);

            }

            return text;
        }


        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(_config["Email:Gmail:Username"] ?? string.Empty),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }

    }
}

