using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.DTOs
{
    public class PersonDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string AddressTitle { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string ClanName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string? FacebookAccount { get; set; }
        public string? InstagramAccount { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string FGrandFatherName { get; set; } = string.Empty;
        public string FGrandMotherName { get; set; } = string.Empty;
        public string MGrandFatherName { get; set; } = string.Empty;
        public string MGrandMotherName { get; set; } = string.Empty;
    }
}
