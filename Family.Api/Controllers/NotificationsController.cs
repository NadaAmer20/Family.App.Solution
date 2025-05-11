using Family.Api.Helpers;
using Family.Core.DTOs;
using Family.Core.Entities;
using Family.Core.Repository.Interfaces;
using Family.Core.Specifications.NotificationSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Family.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IGenericRepository<Notifications> _notificationsRepo;

        public NotificationsController(IGenericRepository<Notifications> notificationsRepo)
        {
            _notificationsRepo = notificationsRepo;
        }

        // GET: api/Notifications/person/{personId}
        [HttpGet("person/{personId}")]
        public async Task<ActionResult<IEnumerable<NotificationsDto>>> GetPersonNotifications(int personId)
        {
            var spec = new NotificationSpecification(personId, true);
            var notifications = await _notificationsRepo.ListAsync(spec);

            // Order by creation date descending (newest first)
            var orderedNotifications = notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToDtos();

            return Ok(orderedNotifications);
        }

        //// GET: api/Notifications/latest/person/{personId}
        //[HttpGet("latest/person/{personId}")]
        //public async Task<ActionResult<IEnumerable<Notifications>>> GetLatestNotifications(int personId)
        //{
        //    var spec = new NotificationSpecification().GetLatestNotifications(personId);
        //    var notifications = await _notificationsRepo.ListAsync(spec);
        //    return Ok(notifications);
        //}

        // GET: api/Notifications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Notifications>> GetNotification(int id)
        {
            var spec = new NotificationSpecification(id);
            var notification = await _notificationsRepo.GetBySpecification(spec);

            if (notification == null)
                return NotFound($"Notification with ID {id} not found");

            return Ok(notification);
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<ActionResult<Notifications>> CreateNotification([FromBody] Notifications notification)
        {
            if (notification == null)
                return BadRequest();

            // Set creation date
            notification.CreatedAt = DateTime.UtcNow;

            await _notificationsRepo.AddAsync(notification);
            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        // DELETE: api/Notifications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var spec = new NotificationSpecification(id);
            var notification = await _notificationsRepo.GetBySpecification(spec);

            if (notification == null)
                return NotFound($"Notification with ID {id} not found");

            await _notificationsRepo.DeleteAsync(notification);
            return NoContent();
        }

        // DELETE: api/Notifications/person/{personId}
        [HttpDelete("person/{personId}")]
        public async Task<IActionResult> DeleteAllPersonNotifications(int personId)
        {
            var spec = new NotificationSpecification(personId, true);
            var notifications = await _notificationsRepo.ListAsync(spec);

            foreach (var notification in notifications)
            {
                await _notificationsRepo.DeleteAsync(notification);
            }

            return NoContent();
        }
    }
}
