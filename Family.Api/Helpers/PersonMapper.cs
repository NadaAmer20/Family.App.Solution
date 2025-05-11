using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class PersonMapper
    {
        public static PersonDto ToDto(this Person entity)
        {
            return new PersonDto
            {
                Id = entity.Id,
                Name = entity.Name,
                PhotoUrl = entity.PhotoUrl,
                FatherName = entity.FatherName,
                MotherName = entity.MotherName,
                FGrandFatherName = entity.FGrandFatherName?? string.Empty,
                FGrandMotherName = entity.FGrandMotherName ?? string.Empty,
                MGrandFatherName = entity.MGrandFatherName ?? string.Empty,
                MGrandMotherName = entity.MGrandMotherName ?? string.Empty,
                EmailAddress = entity.EmailAddress,
                AddressTitle = entity.AddressTitle ?? string.Empty,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                ClanName = entity.Clan?.Name ?? string.Empty,
                BranchName = entity.Branch?.Name ?? string.Empty
            };
        }

        public static IEnumerable<PersonDto> ToDtos(this IEnumerable<Person> entities)
        {
            return entities.Select(e => e.ToDto());
        }

    }
}
