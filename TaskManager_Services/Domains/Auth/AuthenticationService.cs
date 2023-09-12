using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Data.Interfaces;
using TaskManager_Models.Entities.Domains.Auth;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Entities.Enums;
using TaskManager_Models.Exceptions;
using TaskManager_Services.Domains.Auth.Dtos;
using TaskManager_Services.Extensions;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtConfig _jwtConfig;

        private readonly TimeSpan RefreshTokenValidity = TimeSpan.FromDays(7);

        public AuthenticationService(
              IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            JwtConfig jwtConfig
          )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = jwtConfig;
        }
        public async Task<ServiceResponse<string>> SignUp(SignUpDto model)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    throw new AuthException($"Account with Email {model.Email} already exist");
                }
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    throw new AuthException($"Account with Username {model.UserName} already exist");
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    throw new AuthException($"Password does not match");
                }

                user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                IdentityResult userResult = await _userManager.CreateAsync(user, model.Password);
                if (!userResult.Succeeded)
                {
                    var failureReason = userResult.Errors.First().Description;
                    throw new AuthException($"Failed to create user: {failureReason}");
                }

                if (!await _roleManager.RoleExistsAsync(UserType.User.ToString()))
                {
                    userResult = await _roleManager.CreateAsync(new IdentityRole(UserType.User.ToString()));
                    if (!userResult.Succeeded)
                    {
                        string? message = userResult.Errors.Select(e => e.Description).FirstOrDefault();
                        return new ServiceResponse<string>
                        {
                            Message = $"Account creation failed with reason: {message}",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }
                else
                {
                    userResult = await _userManager.AddToRoleAsync(user, UserType.User.ToString());
                    if (!userResult.Succeeded)
                    {
                        string? message = userResult.Errors.Select(e => e.Description).FirstOrDefault();
                        return new ServiceResponse<string>
                        {
                            Message = $"Account creation failed with reason: {message}",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                return new ServiceResponse<string>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "user created, check your email to confirmation"
                };

            }
            catch (AuthException ex)
            {
                return new ServiceResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }
        public Task<ServiceResponse<UserSignedInDto>> GoogleSignUp()
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResponse<UserSignedInDto>> SignIn(SignInDto model)
        {
            try
            {

                ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return new ServiceResponse<UserSignedInDto>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = $"Invalid login credentials"
                    };
                }

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isPasswordCorrect)
                {
                    return new ServiceResponse<UserSignedInDto>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = $"Invalid login credentials"
                    };
                }
                var result = await CreateAccessTokenAsync(user);

                return new ServiceResponse<UserSignedInDto>()
                {
                    Data = result,
                    Message = $"Login Successful",
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserSignedInDto>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = ex.Message
                };
            }
        }
        public async Task<ServiceResponse> ConfirmEmailAsync(ConfirmEmailDto request, CancellationToken cancellationToken)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    var result = await _userManager.ConfirmEmailAsync(user, request.Otp);
                    if (result.Succeeded)
                    {
                        return new ServiceResponse
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Email Verified Succefully!"
                        };
                    }
                }
                return new ServiceResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Account not found"
                };



            }
            catch (AuthenticationException ex)
            {
                await _unitOfWork.SaveChangesAsync();
                return new ServiceResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }
        public async Task<ServiceResponse<UserSignedInDto>> RefreshAccessTokenAsync(string accessToken, string refreshToken)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal.Identity == null)
                {
                    throw new AuthException("Access has expired");
                }

                string email = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new AuthException("Access has expired");
                }
                var tokenData = await _unitOfWork.GetRepository<RefreshToken>().SingleAsync(r => r.Token == refreshToken);

                if (tokenData == null)
                {
                    throw new AuthException("Access has expired");
                }

                bool isRefreshTokenInValid = tokenData.Token != refreshToken || tokenData.ExpiryDate <= DateTime.UtcNow;

                if (isRefreshTokenInValid)
                {
                    throw new AuthException("Access has expired");
                }

                var result = await CreateAccessTokenAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceResponse<UserSignedInDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserSignedInDto>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<UserSignedUpDto>> SignUpWithToken(SignUpDto model)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    throw new AuthException($"Account with Email {model.Email} already exist");
                }
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    throw new AuthException($"Account with Username {model.UserName} already exist");
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    throw new AuthException($"Password does not match");
                }

                user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),

                };

                IdentityResult userResult = await _userManager.CreateAsync(user, model.Password);
                if (userResult.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    return new ServiceResponse<UserSignedUpDto>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "user created, check your email to confirmation",
                        Data = new UserSignedUpDto(token, user)
                    };
                }

                if (!await _roleManager.RoleExistsAsync(UserType.User.ToString()))
                {
                    userResult = await _roleManager.CreateAsync(new IdentityRole(UserType.User.ToString()));
                    if (!userResult.Succeeded)
                    {
                        string? message = userResult.Errors.Select(e => e.Description).FirstOrDefault();
                        return new ServiceResponse<UserSignedUpDto>
                        {
                            Message = $"Account creation failed with reason: {message}",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }
                else
                {
                    userResult = await _userManager.AddToRoleAsync(user, UserType.User.ToString());
                    if (!userResult.Succeeded)
                    {
                        string? message = userResult.Errors.Select(e => e.Description).FirstOrDefault();
                        return new ServiceResponse<UserSignedUpDto>
                        {
                            Message = $"Account creation failed with reason: {message}",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }
                return new ServiceResponse<UserSignedUpDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "failed to create"
                };



            }
            catch (AuthException ex)
            {
                return new ServiceResponse<UserSignedUpDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            string secretKey = _jwtConfig.JwtKey;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = true,
                ValidIssuer = _jwtConfig.JwtIssuer,
                ValidAudience = _jwtConfig.JwtAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
                )
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        private async Task<UserSignedInDto> CreateAccessTokenAsync(ApplicationUser? user)
        {
            if (user == null)
            {
                throw new AuthenticationException("Invalid login credentials");
            }

            JwtSecurityTokenHandler jwTokenHandler = new JwtSecurityTokenHandler();
            string? secretKey = _jwtConfig.JwtKey;
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            var issuer = _jwtConfig.JwtIssuer;
            var audience = _jwtConfig.JwtAudience;

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.JwtExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = issuer,
                Audience = audience
            };

            var securityToken = jwTokenHandler.CreateToken(tokenDescriptor);
            var accessToken = jwTokenHandler.WriteToken(securityToken);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            await _unitOfWork.SaveChangesAsync();

            return new UserSignedInDto(accessToken, refreshToken, tokenDescriptor.Expires.Value.ToTimeStamp());
        }
        private async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                IEnumerable<RefreshToken> refreshTokens = await _unitOfWork.GetRepository<RefreshToken>()
                                                                     .GetQueryableList(x => x.UserId == userId && x.IsActive).ToListAsync();

                foreach (var token in refreshTokens)
                {
                    token.IsActive = false;
                }
                _unitOfWork.GetRepository<RefreshToken>().Add(new RefreshToken
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.Add(RefreshTokenValidity),
                    IsActive = true,
                    UserId = userId,
                    Token = refreshToken
                });
                return refreshToken;
            }
        }
    }
}
