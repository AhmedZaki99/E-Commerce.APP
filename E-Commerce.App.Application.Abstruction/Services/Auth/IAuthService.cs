using E_Commerce.App.Application.Abstruction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Services.Auth
{
    public interface IAuthService
    {

        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsunc(RegisterDto registerDto);
        Task VerifyEmail(VerifyOtpDto dto);
        Task ResendOTP(ForgatPasswordDto dto);

        Task ForgotPasswordAsync(ForgatPasswordDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto,string otp);
        Task<UserDto> ExternalLoginAsync(string email, ClaimsPrincipal? principal = null);
    }
}
