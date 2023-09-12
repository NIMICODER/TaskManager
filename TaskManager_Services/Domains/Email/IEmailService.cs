using TaskManager_Models.Entities.Domains.Emails;

namespace TaskManager_Services.Domains.Email
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
