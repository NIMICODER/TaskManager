using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Models.Entities.Domains.Security
{
    /// <summary>
    /// Represents the different OTP operations we support
    /// </summary>
    public enum OtpOperation
    {
        /// <summary>
        /// An operation to confirma users email via 2FA OTP code
        /// </summary>
        EmailConfirmation = 1,
        /// <summary>
        /// An operation to reset a users password via 2FA OTP code
        /// </summary>
        PasswordReset = 2,
    }
}
