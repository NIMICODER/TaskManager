using System.ComponentModel.DataAnnotations;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public class SignUpDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid Email Address")]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
    }

}
