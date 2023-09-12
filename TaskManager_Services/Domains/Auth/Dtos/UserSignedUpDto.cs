using Microsoft.AspNetCore.Identity;
using TaskManager_Models.Utility;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record UserSignedUpDto(string Token, IdentityUser User) : BaseRecord;

}
