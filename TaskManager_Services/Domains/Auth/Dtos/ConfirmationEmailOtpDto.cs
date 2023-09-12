using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record ConfirmationEmailOtpDto([Required] string Email);

}
