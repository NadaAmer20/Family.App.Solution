using Family.Core.DTOs;
using Family.Core.Entities;

namespace Family.Api.Helpers
{
    public static class NotificationsMapper
    {
        public static NotificationsDto ToDto(this Notifications entity)
        {
            
            return new NotificationsDto
            {
                Id = entity.Id,
                PersonId = entity.PersonId ?? "",
                Title = entity.Title,
                Body = entity.Body,
                CreatedAt = entity.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                NotificationType = entity.NotificationType,
                IsRead = entity.IsRead,
                Data = entity.Data,
                ImageUrl = entity.ImageUrl,
                RedirectUrl = entity.RedirectUrl,
                IsSent = entity.IsSent,
                SentAt = entity.SentAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                PersonName = entity.Person?.DisplayName ?? string.Empty
            };
        }

        public static IEnumerable<NotificationsDto> ToDtos(this IEnumerable<Notifications> entities)
        {
            return entities.Select(e => e.ToDto());
        }
    }
}
