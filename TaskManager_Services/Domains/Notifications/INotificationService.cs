using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Services.Domains.Notifications.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Notifications
{
    public interface INotificationService
    {
        /// <summary>
        /// Sends out otp emails to users
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateOtpNotificationAsync(CreateOtpNotificationDto model, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<NotificationDto>>> GetNotificationsForUserAsync(Guid userId);
    }
}
