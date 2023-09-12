using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Enums;

namespace TaskManager_Services.Domains.Notifications.Dtos
{
    public record NotificationDto(int NotificationId, NotificationType Type, string Message, DateTime Timestamp);
}
