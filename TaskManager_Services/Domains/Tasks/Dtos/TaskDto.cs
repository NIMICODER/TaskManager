using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Enums;
using TaskManager_Models.Utility;

namespace TaskManager_Services.Domains.Tasks.Dtos
{
    public record TaskDto(Guid TaskId, string Title, string Description, DateTime DueDate, TaskPriority Priority, TaskStatus Status) : BaseRecord;

    public record TaskCreateDto(string Title, string Description, DateTime DueDate, TaskPriority Priority) : BaseRecord;

    public record TaskUpdateDto(string Title, string Description, DateTime DueDate, TaskPriority Priority);
}
