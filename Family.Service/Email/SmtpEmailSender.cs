using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Family.Core.Services.Interfaces;
using Family.Service.DTOS;

namespace Family.Service.Email
{

    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly SmtpClient _smtpClient;

        private readonly EmailSettings _emailSettings;

        public SmtpEmailSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;

            // Initialize SmtpClient
            _smtpClient = new SmtpClient(_emailSettings.SMTPHost, _emailSettings.SMTPPort)
            {
                Credentials = new NetworkCredential(_emailSettings.SMTPUsername, _emailSettings.SMTPPassword),
                EnableSsl = _emailSettings.EnableSSL
            };
        }
 
        public SmtpEmailSender(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SMTPHost"];
            _smtpPort = int.Parse(configuration["EmailSettings:Port"]);
            _smtpUsername = configuration["EmailSettings:SMTPUsername"];
            _smtpPassword = configuration["EmailSettings:SMTPPassword"];
            _enableSsl = bool.Parse(configuration["EmailSettings:EnableSSL"]);
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _fromName = configuration["EmailSettings:FromName"];
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    smtpClient.EnableSsl = _enableSsl;

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}