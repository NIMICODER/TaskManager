namespace TaskManager_Models.Entities.Enums
{
    public enum NotificationType
    {
        /// <summary>
        /// For notifying of due date
        /// </summary>
        DueDateReminder = 1,
        /// <summary>
        /// For notifying of status update
        /// </summary>
        StatusUpdate,
        /// <summary>
        /// For notifying of tasks assignment to either user or project
        /// </summary>
        Assignment,
        /// <summary>
        /// For sending reset password link via email
        /// </summary>
        ResetPassword,
        /// <summary>
        /// For send sending email confirmation link 
        /// </summary>
        ConfirmEmail
    }
}
