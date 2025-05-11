using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Service.DTOS
{
    using System;
    using System.Collections.Generic;

    namespace Family.Service.DTOS
    {
        // Common base DTO
        public abstract class BaseDto
        {
            public int Id { get; set; }
        }

        // Person DTOs
        public class PersonCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string FatherName { get; set; } = string.Empty;
            public string MotherName { get; set; } = string.Empty;
            public DateTime BirthDate { get; set; }
            public string PhoneNumber { get; set; } = string.Empty;
            public string? FacebookAccount { get; set; }
            public string? InstagramAccount { get; set; }
            public string? FGrandFatherName { get; set; }
            public string? FGrandMotherName { get; set; }
            public string? MGrandFatherName { get; set; }
            public string? MGrandMotherName { get; set; }
            public string EmailAddress { get; set; } = string.Empty;
            public string? AddressTitle { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string? FCMToken { get; set; }
            public int ClanId { get; set; }
            public int BranchId { get; set; }
        }

        public class PersonUpdateDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string FatherName { get; set; } = string.Empty;
            public string MotherName { get; set; } = string.Empty;
            public DateTime BirthDate { get; set; }
            public string PhoneNumber { get; set; } = string.Empty;
            public string? FacebookAccount { get; set; }
            public string? InstagramAccount { get; set; }
            public string? FGrandFatherName { get; set; }
            public string? FGrandMotherName { get; set; }
            public string? MGrandFatherName { get; set; }
            public string? MGrandMotherName { get; set; }
            public string EmailAddress { get; set; } = string.Empty;
            public string? AddressTitle { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string? FCMToken { get; set; }
            public int ClanId { get; set; }
            public int BranchId { get; set; }
        }

        public class PersonReturnDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string FatherName { get; set; } = string.Empty;
            public string MotherName { get; set; } = string.Empty;
            public DateTime BirthDate { get; set; }
            public string PhoneNumber { get; set; } = string.Empty;
            public string? FacebookAccount { get; set; }
            public string? InstagramAccount { get; set; }
            public string? FGrandFatherName { get; set; }
            public string? FGrandMotherName { get; set; }
            public string? MGrandFatherName { get; set; }
            public string? MGrandMotherName { get; set; }
            public string EmailAddress { get; set; } = string.Empty;
            public string? AddressTitle { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public ClanSimpleDto Clan { get; set; }
            public BranchSimpleDto Branch { get; set; }
        }

        public class PersonSimpleDto : BaseDto
        {
            public string Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
        }

        // Clan DTOs
        public class ClanCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
        }

        public class ClanUpdateDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
        }

        public class ClanReturnDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public IEnumerable<BranchSimpleDto> Branches { get; set; }
             public IEnumerable<PersonSimpleDto> Members { get; set; }

        }

        public class ClanSimpleDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
        }

        // Branch DTOs
        public class BranchCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public int ClanId { get; set; }
        }

        public class BranchUpdateDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public int ClanId { get; set; }
        }

        public class BranchReturnDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public ClanSimpleDto Clan { get; set; }
            public IEnumerable<PersonSimpleDto> Members { get; set; }
        }

        public class BranchSimpleDto : BaseDto
        {
            public string Name { get; set; } = string.Empty;
        }

        // Assignment DTOs
        public class ClanAssignmentDto
        {
            public int PersonId { get; set; }
            public int ClanId { get; set; }
        }

        public class BranchAssignmentDto
        {
            public int PersonId { get; set; }
            public int BranchId { get; set; }
        }
        // Add this to your Family.Service.DTOS namespace
        public class AppUserSimpleDto : BaseDto
        {
            public string Id { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string PhotoUrl { get; set; } = string.Empty;
            // Add any other properties you need from AppUser
        }
    }
}
