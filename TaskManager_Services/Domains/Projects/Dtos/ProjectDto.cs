using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Enums;
using TaskManager_Models.Utility;

namespace TaskManager_Services.Domains.Projects.Dtos
{
    public record ProjectDto(Guid Id, string Name, string Description) : BaseRecord;

    public record ProjectCreateDto(Guid Id, string Name, string Description);

    public record ProjectUpdateDto(string Name, string Description);

}
