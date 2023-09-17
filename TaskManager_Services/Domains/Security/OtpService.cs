using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Cache.Interfaces;
using TaskManager_Models.Entities.Domains.Security;
using TaskManager_Services.Domains.Security.Dtos;

namespace TaskManager_Services.Domains.Security
{
    public class OtpService : IOtpService
    {
        private readonly ICacheService _cacheService;
        private TimeSpan OtpValidity = TimeSpan.FromMinutes(5);

        public OtpService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<string> GenerateOtpAsync(string userId, OtpOperation operation)
        {
            string cacheKey = CacheKeySelector.OtpCodeCacheKey(userId, operation);
            OtpCodeDto? otpCode = await _cacheService.ReadFromCache<OtpCodeDto>(cacheKey);
            if (otpCode != null)
            {
                otpCode.Otp = GenerateToken();
            }
            else
            {
                otpCode = new(GenerateToken(), userId);
            }
            await _cacheService.WriteToCache(cacheKey, otpCode, null, OtpValidity);
            return otpCode.Otp;
        }

        public async Task<bool> VerifyOtpAsync(string userId, string otp, OtpOperation operation)
        {
            string cacheKey = CacheKeySelector.OtpCodeCacheKey(userId, operation);
            OtpCodeDto? otpCode = await _cacheService.ReadFromCache<OtpCodeDto>(cacheKey);

            if (otpCode == null)
            {
                return false;
            }

            ++otpCode.Attempts;

            if (otpCode.Attempts >= 3)
            {
                await _cacheService.ClearFromCache(cacheKey);
                return false;
            }

            if (otpCode.Otp != otp)
            {
                return false;
            }
            return true;
        }

        private static string GenerateToken()
        {
            Random generator = new Random();
            string token = generator.Next(0, 999999).ToString("D6");

            return token;
        }
    }
}
