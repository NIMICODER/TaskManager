using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Services.Domains.Notifications.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Notifications
{
    public class NotificationService : INotificationService
    {
        public Task<ServiceResponse<NotificationDto>> CreateNotificationAsync(NotificationCreateDto notificationDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotificationAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<NotificationDto>> GetNotificationAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<NotificationDto>>> GetNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<NotificationDto>>> GetNotificationsForUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkNotificationAsReadAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkNotificationAsUnreadAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
