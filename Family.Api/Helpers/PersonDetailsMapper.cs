using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class PersonDetailsMapper
    {
        public static PersonDetailsDto ToDetailsDto(this Person entity)
        {
            var age = CalculateAge(entity.BirthDate);

            return new PersonDetailsDto
            {
                Id = entity.Id,
                Name = entity.Name,
                PhotoUrl = entity.PhotoUrl,

                // Family Information
                FatherName = entity.FatherName,
                MotherName = entity.MotherName,
                FGrandFatherName = entity.FGrandFatherName ?? string.Empty,
                FGrandMotherName = entity.FGrandMotherName ?? string.Empty,
                MGrandFatherName = entity.MGrandFatherName ?? string.Empty,
                MGrandMotherName = entity.MGrandMotherName ?? string.Empty,

                // Personal Information
                BirthDate = entity.BirthDate.ToString("yyyy-MM-dd"),
                Age = $"{age} سنة",
                EmailAddress = entity.EmailAddress,

                // Location Information
                AddressTitle = entity.AddressTitle ?? string.Empty,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,

                // Family Tree Information
                ClanName = entity.Clan?.Name ?? string.Empty,
                BranchName = entity.Branch?.Name ?? string.Empty,

                // Social Media & Contact
                PhoneNumber = entity.PhoneNumber,
                FacebookAccount = entity.FacebookAccount,
                InstagramAccount = entity.InstagramAccount
            };
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }

        public static IEnumerable<PersonDetailsDto> ToDetailsDtos(this IEnumerable<Person> entities)
        {
            return entities.Select(e => e.ToDetailsDto());
        }

    }
}
