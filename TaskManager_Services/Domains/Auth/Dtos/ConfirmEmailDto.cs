using System.ComponentModel.DataAnnotations;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record ConfirmEmailDto([Required] string Email, [Required] string Otp);

}
