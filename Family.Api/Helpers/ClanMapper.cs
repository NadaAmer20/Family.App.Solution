using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class ClanMapper
    {
        public static ClanDto ToDto(this Clan entity)
        {
            return new ClanDto
            {
                Id = entity.Id,
                Name = entity.Name,
                PhotoUrl = entity.PhotoUrl,
                Region = entity.Region,

            };
        }

        public static IEnumerable<ClanDto> ToDtos(this IEnumerable<Clan> entities)
        {
            return entities.Select(e => e.ToDto());
        }

    }
}
