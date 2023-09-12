using System.ComponentModel.DataAnnotations;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record ForgotPasswordOtpDto([Required] string Email);

}
