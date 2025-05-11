using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.DTOs
{
    public class NotificationsDto
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string NotificationType { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public string? Data { get; set; }
        public string? ImageUrl { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsSent { get; set; }
        public string? SentAt { get; set; }
        public string PersonName { get; set; } = string.Empty;

    }
}
