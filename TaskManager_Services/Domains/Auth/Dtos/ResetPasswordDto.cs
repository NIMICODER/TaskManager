using System.ComponentModel.DataAnnotations;
using TaskManager_Models.Utility;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record ResetPasswordDto([Required] string Email, [Required] string Password, [Required] string ConfirmPassword, [Required] string Otp) : BaseRecord;
}
