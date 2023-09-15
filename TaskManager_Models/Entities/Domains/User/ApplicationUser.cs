using Microsoft.AspNetCore.Identity;
using TaskManager_Models.Entities.Domains.Notifications;
using TaskManager_Models.Entities.Domains.Projects;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Enums;

namespace TaskManager_Models.Entities.Domains.User
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public UserType UserTypeId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TaskTodo> Tasks { get; set; }
       // public virtual ICollection<UserTaskAssignment> AssignedTasks { get; set; } = new List<UserTaskAssignment>();
    }
}
