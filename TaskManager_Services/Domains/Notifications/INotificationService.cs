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
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task<ServiceResponse<NotificationDto>> GetNotificationAsync(Guid notificationId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<NotificationDto>>> GetNotificationsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<NotificationDto>> CreateNotificationAsync(NotificationCreateDto notificationDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task<bool> MarkNotificationAsReadAsync(Guid notificationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task<bool> MarkNotificationAsUnreadAsync(Guid notificationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task<bool> DeleteNotificationAsync(Guid notificationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<NotificationDto>>> GetNotificationsForUserAsync(Guid userId);
    }
}
