using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Services.Domains.User.Dtos
{
    public record UserDto(Guid UserId, string Name, string Email);
}
