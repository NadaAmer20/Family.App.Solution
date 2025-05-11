using Family.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Entities
{
    public class Notifications : BaseEntity
    {
        public string? PersonId { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // New FCM-specific properties
        public string NotificationType { get; set; } = string.Empty;  // To differentiate types of notifications
        public bool IsRead { get; set; } = false;                    // Track if notification was read
        public string? Data { get; set; }                           // Additional JSON data for FCM payload
        public string? ImageUrl { get; set; }                       // Optional: URL for notification image
        public string? RedirectUrl { get; set; }                    // Optional: Deep link URL
        public bool IsSent { get; set; } = false;                   // Track if notification was sent to FCM
        public DateTime? SentAt { get; set; }                       // When the notification was sent
        public string? FCMResponse { get; set; }                    // Store FCM response/message ID

        // Navigation property (existing)
        public AppUser? Person { get; set; } = null!;

    }
}
