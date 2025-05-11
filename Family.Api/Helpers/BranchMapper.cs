using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class BranchMapper
    {
        public static BranchDto ToDto(this Branch entity)
        {
            return new BranchDto
            {
                Id = entity.Id,
                Name = entity.Name,
                PhotoUrl = entity.PhotoUrl,
                Region = entity.Region
            };
        }

        public static IEnumerable<BranchDto> ToDtos(this IEnumerable<Branch> entities)
        {
            return entities.Select(e => e.ToDto());
        }
    }
}
