using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Services.Domains.Security.Dtos
{
    public record OtpCodeDto
    {
        /// <summary>
        /// Generated Otp code
        /// </summary>
        public string Otp { get; set; }
        /// <summary>
        /// Identifier of the user that generated the otp code
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Number of failed attempts to match the correct generated otp code
        /// </summary>
        public int Attempts { get; set; }

        public OtpCodeDto(string otp, string userId)
        {
            Otp = otp;
            UserId = userId;
            Attempts = 0;
        }
    }

}
