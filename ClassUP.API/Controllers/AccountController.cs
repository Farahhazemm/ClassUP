using ClassUP.ApplicationCore.DTOs.Requests.Account.Password;
using ClassUP.ApplicationCore.DTOs.Requests.Account_Management;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Refresh;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IUserTokenService _userTokenService;
        public AccountController(IAuthService authservice , UserManager<AppUser> userManager,IUserTokenService userTokenService)
        {
            _authservice = authservice;
            _userTokenService = userTokenService;
        }

        #region Register&Login&LogOut
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var user = await _authservice.RegisterAsync(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authservice.LoginAsync(dto);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _userTokenService.RevokeAllAsync(userId);

            Response.Cookies.Delete("refreshtoken");

            return NoContent();
        }

        #endregion


        #region TokenEndPoints
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshtoken = Request.Cookies["refreshtoken"];

            var result = await _userTokenService.RefreshTokenAsync(refreshtoken);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDTO? dto)
        {
            var token = dto?.Token ?? Request.Cookies["refreshtoken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token Is required");

            var result = await _userTokenService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token Is invalid");

            return Ok(result);
        }

       


        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieoptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),

            };
            Response.Cookies.Append("refreshtoken", refreshToken, cookieoptions);
        }

        #endregion


        #region ForgotPassword
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            await _authservice.SendResetPasswordCode(dto.Email);
            return NoContent();
        }
        #endregion

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            await _authservice.ResetPasswordAsync(dto);
            return NoContent();
        }







    }
}
