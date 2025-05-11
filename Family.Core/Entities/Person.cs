using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Entities
{
    public class Person : BaseEntity
    {        // Existing properties

        public int ClanId { get; set; }
        public int BranchId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? FacebookAccount { get; set; }
        public string? InstagramAccount { get; set; }

        // New properties
        public string? FGrandFatherName { get; set; } = string.Empty; // Father's father
        public string? FGrandMotherName { get; set; } = string.Empty; // Father's mother
        public string? MGrandFatherName { get; set; } = string.Empty; // Mother's father
        public string? MGrandMotherName { get; set; } = string.Empty; // Mother's mother
        public string EmailAddress { get; set; } = string.Empty;
        public string? AddressTitle { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? FCMToken { get; set; }

        // Navigation properties
        public Clan Clan { get; set; } = null!;
        public Branch Branch { get; set; } = null!;
        public ICollection<Notifications> Notifications { get; set; } = new HashSet<Notifications>();

    }
}
