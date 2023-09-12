using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Controllers.V1.Shared;
using TaskManager_Services.Domains.Notifications.Dtos;

namespace TaskManager_API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BaseController
    {

        [HttpGet]
        public IActionResult GetNotifications()
        {
            return Ok("OK");
        }

        [HttpGet("{id}")]
        public IActionResult GetNotification(Guid id)
        {
            return Ok("OK");
        }

        [HttpPost]
        public IActionResult CreateNotification([FromBody] NotificationDto notificationDto)
        {
            return Ok("OK");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotification(Guid id, [FromBody] NotificationDto notificationDto)
        {
            return Ok("OK");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(Guid id)
        {
            return Ok("OK");
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public IActionResult MarkNotificationAsRead(Guid notificationId)
        {
            return Ok("OK");
        }

        [HttpPut("mark-as-unread/{notificationId}")]
        public IActionResult MarkNotificationAsUnread(Guid notificationId)
        {
            return Ok("OK");
        }


    }
}
