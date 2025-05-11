using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class SliderMapper
    {
        public static SliderItemDto ToDto(this SliderItem entity)
        {
            return new SliderItemDto
            {
                Id = entity.Id,
                PhotoUrl = entity.PhotoUrl,
                Description = entity.Description
            };
        }

        public static IEnumerable<SliderItemDto> ToDtos(this IEnumerable<SliderItem> entities)
        {
            return entities.Select(e => e.ToDto());
        }
    }
}
