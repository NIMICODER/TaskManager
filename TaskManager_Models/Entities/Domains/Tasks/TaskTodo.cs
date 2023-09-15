using System.ComponentModel.DataAnnotations.Schema;
using TaskManager_Models.Entities.Domains.Projects;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Entities.Enums;

namespace TaskManager_Models.Entities.Domains.Tasks
{
    public class TaskTodo : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }

        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        //[ForeignKey("UserTaskAssignment")]
        //public string UserId { get; set; }
        //public virtual ICollection<UserTaskAssignment> AssignedUsers { get; set; } = new List<UserTaskAssignment>();
    }

    public class UserTaskAssignment
    {
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid TaskId { get; set; }
        public virtual TaskTodo Task { get; set; }
    }

}
