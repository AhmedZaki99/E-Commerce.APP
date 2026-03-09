using E_Commerce.APIs.Controllers.Base;
using E_Commerce.App.Application.Abstruction.Models.Auth;
using E_Commerce.App.Application.Abstruction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Api.Controller.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager) : BaseApiController
    {

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsunc(model);
            return Ok(result);
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetCurrentUser()
        //{
        //    var result = await serviceManager.AuthService.GetCurrentUser(User);
        //    return Ok(result);
        //}

        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPassword(ForgatPasswordDto model)
        {
            //var url = Url.Action(
            //    "ResetPassword",       // Action
            //    "Account",             // Controller
            //    values: new {Email = model.Email},          // Query params مش هنا
            //    protocol: Request.Scheme, // http أو https
            //    host: Request.Host.Value // hostname + port

            //    );
            await serviceManager.AuthService.ForgotPasswordAsync(model);
            return Ok("Check Your Mail");

        }


        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            await serviceManager.AuthService.ResetPasswordAsync(model);
            return Ok("Password Saved");
        }

        [HttpPost("CheckOtp")]
        public async Task<ActionResult> VerifyOtp(VerifyOtpDto model)
        {
            await serviceManager.AuthService.CheckOtp(model);
            return Ok("OTP is true");
        }

        [HttpPost("VerifyEmail")]
        public async Task<ActionResult> VerifyEmail(VerifyOtpDto model)
        {
            await serviceManager.AuthService.VerifyEmail(model);
            return Ok("Email verified successfully");

        }

        [HttpPost("ResendOtp")]
        public async Task<ActionResult> ResendOTP(ForgatPasswordDto model)
        {
            await serviceManager.AuthService.ResendOTP(model);
            return Ok("OTP resent successfully");
        }

        //[HttpGet("signin-google")]
        //public ActionResult SignInWithGoogle()
        //{
        //    var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        //    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        //}
         
        //[HttpGet("google-response")]
        //public async Task<ActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        //    if (!result.Succeeded)
        //        return BadRequest("Google authentication failed");
         
        //   var Claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(
        //       claim => new
        //       {
        //           claim.Issuer,
        //           claim.OriginalIssuer,
        //             claim.Type,
        //             claim.Value
        //       });
         
        //    return RedirectToAction("GetProducts", "Product");
        //}
    }
}
