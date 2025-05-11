using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class PhotoMapper
    {
        public static PhotoDto ToDto(this Photo entity)
        {
            return new PhotoDto
            {
                Id = entity.Id,
                Title = entity.Title, // Map the new Title property
                PhotoUrl = entity.PhotoUrl,
                Description = entity.Description,
                DateTaken = entity.DateTaken.ToString("yyyy/MM/dd") // Format as shown in the design
            };
        }

        public static IEnumerable<PhotoDto> ToDtos(this IEnumerable<Photo> entities)
        {
            return entities.Select(e => e.ToDto());
        }

    }
}
