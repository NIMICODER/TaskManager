using System.ComponentModel.DataAnnotations.Schema;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Entities.Enums;

namespace TaskManager_Models.Entities.Domains.Notifications
{
    public class Notification : BaseEntity
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        //[ForeignKey("Task")]
        //public Guid? TaskId { get; set; }
        //public virtual TaskTodo Task { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
