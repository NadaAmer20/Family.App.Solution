using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.DTOs
{

    public class UserReturnDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
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
    }

    public class UserCreateDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

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
    }

    public class UserUpdateDto
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

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
    }

    public class BranchAssignmentDto
    {
        public string UserId { get; set; } = string.Empty;
        public int BranchId { get; set; }
    }
}
