using System.ComponentModel.DataAnnotations;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record SignInDto([Required] string Email, [Required] string Password);

}
