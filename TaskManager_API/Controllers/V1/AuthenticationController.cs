using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager_API.Controllers.V1.Shared;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Auth;
using TaskManager_Services.Domains.Auth.Dtos;
using TaskManager_Services.Domains.Email;
using TaskManager_Services.Utility;

namespace TaskManager_API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authService;
        private readonly IEmailService _emailService;
        public AuthenticationController(IAuthenticationService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }


        [HttpPost("sign-up")]
        [ProducesResponseType(200, Type = typeof(ApiRecordResponse<UserSignedInDto>))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var response = new ServiceResponse<UserSignedInDto>()
                {
                    Message = errors.FirstOrDefault(),
                    StatusCode = HttpStatusCode.BadRequest
                };
                return ComputeApiResponse(response);
            }

            var result = await _authService.SignUp(model);
            return Ok(result);
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(200, Type = typeof(ApiRecordResponse<UserSignedInDto>))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> SignIn([FromBody] SignInDto model, CancellationToken cancellationToken)
        {
            ServiceResponse<UserSignedInDto> response = await _authService.SignIn(model);
            return ComputeResponse(response);
        }
        //[HttpPost("sign-up-email")]
        //[ProducesResponseType(200, Type = typeof(ApiRecordResponse<UserSignedInDto>))]
        //[ProducesResponseType(404, Type = typeof(ApiResponse))]
        //[ProducesResponseType(400, Type = typeof(ApiResponse))]
        //public async Task<IActionResult> SignUpEmail([FromBody] SignUpDto model)
        //{

        //    var token = await _authService.SignUpWithToken(model);
        //    if (token == null)
        //    {
        //        var confirmationLink = Url.Action(nameof(_authService.ConfirmEmailAsync), "Authentication", new { token, email = model.Email }, Request.Scheme);
        //        var emailMessage = new Message(new string[] { model.Email }, "Confirmation email link", confirmationLink);
        //        _emailService.SendEmail(emailMessage);
        //        return StatusCode(StatusCodes.Status200OK, "Email verified");
        //    }

        //    return ComputeApiResponse(token);


        //}


        [HttpPost("verify-email")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> VerifyEmail([FromBody] ConfirmEmailDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var response = new ServiceResponse()
                {
                    Message = errors.FirstOrDefault(),
                    StatusCode = HttpStatusCode.BadRequest
                };
                return ComputeResponse(response);
            }
            var Result = await _authService.ConfirmEmailAsync(model, CancellationToken.None);
            return ComputeResponse(Result);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Refresh(string accessToken, string refreshToken)
        {
            var tokenResponse = await _authService.RefreshAccessTokenAsync(accessToken, refreshToken);
            return ComputeResponse(tokenResponse);
        }
    }
}
