using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager_AsyncClient.Interfaces;
using TaskManager_Data.Interfaces;
using TaskManager_MessageContracts;
using TaskManager_MessageContracts.Commands.Notification;
using TaskManager_Models.Configuration;
using TaskManager_Models.Entities.Domains.Notifications;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Entities.Enums;
using TaskManager_Services.Domains.Notifications.Dtos;
using TaskManager_Services.Domains.Security;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpService _otpService;
        private readonly ICommandClient _commandClient;
        private readonly QueueConfiguration _queueConfiguration;
        private readonly IRepository<ApplicationUser> _userRepo;
        private readonly IRepository<TaskTodo> _taskRepo;
        private readonly IRepository<Notification> _notificationRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _taskRepo = _unitOfWork.GetRepository<TaskTodo>();
            _notificationRepo = _unitOfWork.GetRepository<Notification>();
            _userRepo = _unitOfWork.GetRepository<ApplicationUser>();
                
        }






        public async Task CreateOtpNotificationAsync(CreateOtpNotificationDto model, CancellationToken cancellationToken)
        {
            string subject = GenerateSubject(model.noteOperation);
            var otp = await _otpService.GenerateOtpAsync(model.userId, model.noteOperation);
            var messageId = SHA256Hasher.Hash($"{typeof(CreateOtpNotificationDto)}_{model.userId}_{DateTime.UtcNow.Date.ToShortDateString()}");
            SendEmailNotification command = new()
            {
                Source = MessagingSource.API,
                Subject = subject,
                CommandSentAt = DateTime.UtcNow,
                Content = new List<string>() { otp },
                IsTransactional = true,
                To = new Personality(model.email, model.fullName),
                TTL = TimeSpan.FromMinutes(5),
                MessageId = messageId
            };

            await _commandClient.SendCommand(_queueConfiguration.NotificationQueueUrl, command, cancellationToken);
        }

        public async Task<ServiceResponse<IEnumerable<NotificationDto>>> GetNotificationsForUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                {
                    return new ServiceResponse<IEnumerable<NotificationDto>>
                    {
                        Data = null,
                        Message = "User not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var notifications = await _notificationRepo.GetAllAsync(include: u => u.Include(u => u.User));

                var notificationDtos = notifications.Select(notification => new NotificationDto
                (
                  notification.Id,
                  notification.Type,
                  notification.Message,
                  notification.Timestamp
                ));

                return new ServiceResponse<IEnumerable<NotificationDto>>
                {
                    Data = notificationDtos,
                    Message = "Notifications fetched successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return new ServiceResponse<IEnumerable<NotificationDto>>
                {
                    Data = null,
                    Message = "Error fetching notifications",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private string GenerateSubject(NotificationType operation)
        {
            switch (operation)
            {
                case NotificationType.DueDateReminder:
                    return "Email Confirmation One-Time Password";
                case NotificationType.StatusUpdate:
                    return "Email Confirmation One-Time Password";
                case NotificationType.Assignment:
                    return "Email Confirmation One-Time Password";
                case NotificationType.ResetPassword:
                    return "Email Confirmation One-Time Password";
                case NotificationType.ConfirmEmail:
                    return "Password Reset One-Time Password";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
    }
}
