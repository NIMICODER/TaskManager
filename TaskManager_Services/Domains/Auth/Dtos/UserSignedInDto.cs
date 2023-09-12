using TaskManager_Models.Utility;

namespace TaskManager_Services.Domains.Auth.Dtos
{
    public record UserSignedInDto(string AccessToken, string RefreshToken, long TokenExpiryTimeStamp) : BaseRecord;

}
