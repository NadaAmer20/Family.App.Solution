using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string messageWeb, Dictionary<string, string> placeholders);
        Task SendEmailAsync(string to, string subject, string htmlMessage);
         
    }
}
