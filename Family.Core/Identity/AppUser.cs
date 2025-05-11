using Family.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Family.Core.Identity
{
    public class AppUser : IdentityUser
    {
        // Identity properties
        public string DisplayName { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
 
        public int? BranchId { get; set; }
        public string? PhotoUrl { get; set; } = string.Empty;
        public string? FatherName { get; set; } = string.Empty;
        public string? MotherName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? FacebookAccount { get; set; }
        public string? InstagramAccount { get; set; }
        public string? FGrandFatherName { get; set; } = string.Empty;
        public string? FGrandMotherName { get; set; } = string.Empty;
        public string? MGrandFatherName { get; set; } = string.Empty;
        public string? MGrandMotherName { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public string? AddressTitle { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? FCMToken { get; set; }

        // Navigation properties
        public virtual Branch? Branch { get; set; }
        public ICollection<Notifications>? Notifications { get; set; }
    }
}