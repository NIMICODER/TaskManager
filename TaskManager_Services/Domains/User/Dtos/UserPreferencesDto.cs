namespace TaskManager_Services.Domains.User.Dtos
{
    public record UserPreferencesDto(int UserId, string Theme, bool EmailNotifications);
}
