using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskTodo = TaskManager_Models.Entities.Domains.Tasks.TaskTodo;
using TaskManager_Models.Entities.Domains.User;

namespace TaskManager_Models.Entities.Domains.Projects
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TaskTodo> Tasks { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
