using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Services.Domains.Tasks.Dtos;

namespace TaskManager_Services.Domains.Date.Dtos
{
    public record DueDateDto(DateTime StartOfWeek, DateTime EndOfWeek, List<TaskDto> TasksDueThisWeek);
}
