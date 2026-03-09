using E_Commerce.App.Application.Abstruction.Models.Auth;
using E_Commerce.App.Application.Abstruction.Services.Auth;
using E_Commerce.App.Application.Exception;
using E_Commerce.App.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Mvc.Core;

namespace E_Commerce.App.Application.Service.Auth
{
    public class AuthService(
        IOptions<JWTSettings> JwtSetting,
        UserManager<ApplicationsUser> userManager,
        SignInManager<ApplicationsUser> signinManger,
        IEmailService _emailService) : IAuthService
    {

        private readonly JWTSettings _jwtSettings = JwtSetting.Value;
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnAuthorizedExeption("invalid login");

            var result = await signinManger.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (result.IsLockedOut) throw new UnAuthorizedExeption("Account is locked out");
            if (result.IsNotAllowed) throw new UnAuthorizedExeption("Account is not allowed to login");


            if (!result.Succeeded) throw new NotFoundException("Invalid Login" , user.Email);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisablayName = user.DisableName,
                Token = await GeneratTokenAsync(user)
            };
        }
        public async Task<UserDto> RegisterAsunc(RegisterDto registerDto)
        {
            var Emailexisted = await userManager.FindByEmailAsync(registerDto.Email);
            if (Emailexisted is not null) throw new UnAuthorizedExeption("Email Is Existe");
          
            var user = new ApplicationsUser
            {
                UserName = registerDto.DisplayName.Replace(" ",""),
                Email = registerDto.Email,
                DisableName = registerDto.DisplayName,
                PhoneNumber = registerDto.Phone
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) throw new ValidationExeption() { Errors = result.Errors.Select(E=>E.Description)};

            user.Otp = /*new Random().Next(1000, 9999).ToString()*/"1234";
            user.OtpExpire = DateTime.UtcNow.AddMinutes(5);

            await userManager.UpdateAsync(user);
            _emailService.SendEmail(user.Email!,
                                    "Verify Your Email – Enjaz MultiVendor",
                                    $@"
                                    Hello {user.UserName},
                                    Welcome to Enjaz MultiVendor! Please verify your email by entering the OTP code below:
                                    {user.Otp}
                                    This code will expire in 10 minutes.
                                    If you did not sign up, please ignore this email.
                                    Thanks,<br/>Enjaz MultiVendor Team "
            );
            
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisablayName = user.DisableName,
                Token = await GeneratTokenAsync(user)
            };

        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal principal)
        {
            var Email =  principal.FindFirstValue(ClaimTypes.Email);
            var user = userManager.FindByEmailAsync(Email!).Result;

            return new UserDto
            {
                Id = user!.Id,
                Email = user.Email!,
                DisablayName = user.DisableName,
                Token = await GeneratTokenAsync(user)
            };
        }

        public async Task ForgotPasswordAsync(ForgatPasswordDto dto /*,string url*/)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is null) throw new NotFoundException("user not found",dto.Email);

            var otp = /*new Random().Next(1000, 9999).ToString()*/ "1234";
            var expierd =  DateTime.UtcNow.AddMinutes(5);

            user.Otp = otp;
            user.OtpExpire = expierd;

            await userManager.UpdateAsync(user);
            var email = new 
            {
                To = user.Email!,
                Subject = "Reset Password",
                Body = 
                $@"
                Hello {user.UserName}
                We received a request to reset your password. Use the OTP code below to reset it:
                {user.Otp}
                This code will expire in 5 minutes.
                If you did not request a password reset, please ignore this email.
                Thanks,Enjaz Application Team"
            };


            _emailService.SendEmail(email.To, email.Subject, email.Body);

        }
        public async Task CheckOtp(VerifyOtpDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new NotFoundException("Email is not Exsist", model.Email);

            if (user.Otp != model.OTP || user.OtpExpire < DateTime.UtcNow)
                throw new UnAuthorizedExeption("Invalid or expired OTP");

            user.Otp = null;
            user.OtpExpire = null;
            await userManager.UpdateAsync(user);
        }
        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            if(dto.NewPassword != dto.ConfirmPassword)
                throw new ValidationExeption() { Errors = new List<string> { "New password and confirm password do not match." } };

            var user =await userManager.FindByEmailAsync(dto.Email);
            
            if (user is null) throw new NotFoundException("user not found", dto.Email);

            //var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            //Console.WriteLine(decodedToken);
           

            await userManager.RemovePasswordAsync(user);

            var result = await userManager.AddPasswordAsync(user, dto.NewPassword);

           

            if (!result.Succeeded)
            
                throw new ValidationExeption() { Errors =  result.Errors.Select(e => e.Description) };
        }
       

        private async Task<string> GeneratTokenAsync(ApplicationsUser user)
        {
            var UserClaims = await userManager.GetClaimsAsync(user);
            var userRoles = new List<Claim>();

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                userRoles.Add(new Claim(ClaimTypes.Role , role.ToString()));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid , user.Id),
                new Claim(ClaimTypes.Email , user.Email!),
                new Claim(ClaimTypes.GivenName , user.DisableName),
            }.Union(userRoles)
            .Union(UserClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var TokenObj = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DuerationInMinutes),
                claims: claims,
                signingCredentials: signinCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(TokenObj);

        }

  
        public async Task VerifyEmail(VerifyOtpDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is null) throw new NotFoundException("user not found", dto.Email);

            if (user.Otp != dto.OTP || user.OtpExpire < DateTime.UtcNow)
                throw new UnAuthorizedExeption("Invalid or expired OTP");

            user.EmailConfirmed = true;
            user.Otp = null;
            user.OtpExpire = null;
            await userManager.UpdateAsync(user);

        }

        public async Task ResendOTP(ForgatPasswordDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is null) throw new NotFoundException("user not found", dto.Email);

            user.Otp = /*new Random().Next(1000, 9999).ToString()*/"1234";
            user.OtpExpire = DateTime.UtcNow.AddMinutes(5);
            await userManager.UpdateAsync(user);

             _emailService.SendEmail(user.Email!,"Resend otp",
                $@"
                Hello {user.UserName}
                We received a request to resend Otp. Use the OTP code below to reset it:
                {user.Otp}
                This code will expire in 5 minutes.
                If you did not request , please ignore this email.
                Thanks,Enjaz Application Team");
        }
       
        public async Task<UserDto> ExternalLoginAsync(string email, ClaimsPrincipal? principal = null)
        {
            //  جلب المستخدم من قاعدة البيانات
            var user = await userManager.FindByEmailAsync(email);

            //  لو المستخدم مش موجود → تسجيل جديد
            if (user == null)
            {
                var name = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? email;

                user = new ApplicationsUser
                {
                    Email = email,
                    DisableName = name,
                    UserName = name.Replace(" ", ""),

                };

                await userManager.CreateAsync(user);
            }

            // 3️⃣ إنشاء JWT Token
            var token = GeneratTokenAsync(user);

            // 4️⃣ تجهيز DTO
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisablayName = user.DisableName,
                Token = await GeneratTokenAsync(user)

            };

            return userDto;
        }

      
    }
}
