using IdentityServer.Application.ImplementServices.UseCases.UserCase;
using IdentityServer.Application.ImplementServices.UseCases.UserCase.Requests;
using IdentityServer.Application.ImplementServices.UseCases.UserCase.Responses.LoginResponse;
using IdentityServer.Application.InterfaceServices;
using IdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUserService _user;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IUserService user,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Request_Register registerUser)
        {
            var tokenResponse = await _user.CreateUserWithTokenAsync(registerUser);
            if (tokenResponse.IsSuccess && tokenResponse.Response != null)
            {
                await _user.AssignRoleToUserAsync(registerUser.Roles, tokenResponse.Response.User);

                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { tokenResponse.Response.Token, email = registerUser.Email }, Request.Scheme);

                var message = new Message(new string[] { registerUser.Email! }, "Link xác nhận email", confirmationLink!);
                var responseMsg = _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK,
                        new Response { IsSuccess = true, Message = $"{tokenResponse.Message} {responseMsg}" });

            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response { Message = tokenResponse.Message, IsSuccess = false });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                      new Response { Status = "Success", Message = "Email Verified Successfully", IsSuccess = true });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                       new Response { Status = "Error", Message = "This User Doesnot exist!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Request_Login loginModel)
        {
            var loginOtpResponse = await _user.GetOtpByLoginAsync(loginModel);
            if (loginOtpResponse.Response != null)
            {
                var user = loginOtpResponse.Response.User;
                if (user.TwoFactorEnabled)
                {
                    var token = loginOtpResponse.Response.Token;
                    var message = new Message(new string[] { user.Email! }, "Mã xác nhận OTP", token);
                    _emailService.SendEmail(message);

                    return StatusCode(StatusCodes.Status200OK,
                     new Response { IsSuccess = loginOtpResponse.IsSuccess, Status = "Thành công", Message = $"Chúng tôi đã gửi mã OTP về email của bạn:  {user.Email}" });
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var serviceResponse = await _user.GetJwtTokenAsync(user);
                    return Ok(serviceResponse);

                }
            }
            return Unauthorized();

        }

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string userName)
        {
            var jwt = await _user.LoginUserWithJWTokenAsync(code, userName);
            if (jwt.IsSuccess)
            {
                return Ok(jwt);
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Thành Công", Message = $"Mã không hợp lệ" });
        }

        [HttpPost]
        [Route("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(DataLoginResponse tokens)
        {
            var jwt = await _user.RenewAccessTokenAsync(tokens);
            if (jwt.IsSuccess)
            {
                return Ok(jwt);
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Thành công", Message = $"Mã không hợp lệ" });
        }
    }
}
