using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Enums;

namespace TaskManager_Services.Domains.Tasks.Dtos
{
    public record TaskDto(Guid TaskId, string Title, string Description, DateTime DueDate, TaskPriority Priority, TaskStatus Status);
}
