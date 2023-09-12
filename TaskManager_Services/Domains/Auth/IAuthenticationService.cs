using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Services.Domains.Auth.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Auth
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Utilizes the email confirmation OTP as a 2FA mechanism in confirming the users email address
        /// </summary>
        /// <param name="model" <see cref="ConfirmEmailDto"/>></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResponse> ConfirmEmailAsync(ConfirmEmailDto model, CancellationToken cancellationToken);

        Task<ServiceResponse<UserSignedInDto>> GoogleSignUp();
        /// <summary>
        /// Generates a new access token using the users refresh token
        /// </summary>
        /// <param name="accessToken">Expired JWT</param>
        /// <param name="refreshToken">Users refresh token</param>
        /// <returns></returns>
        Task<ServiceResponse<UserSignedInDto>> RefreshAccessTokenAsync(string accessToken, string refreshToken);


        /// <summary>
        /// Validates user login credentials and generates an accesstoken and a refresh token
        /// </summary>
        /// <param name="model" <see cref="SignInDto"/>></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResponse<UserSignedInDto>> SignIn(SignInDto model);

        /// <summary>
        /// Handles creation of new user accounts
        /// </summary>
        /// <param name="model" <see cref="SignUpDto"/>></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> SignUp(SignUpDto model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResponse<UserSignedUpDto>> SignUpWithToken(SignUpDto model);
    }
}
