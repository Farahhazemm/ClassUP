using ClassUP.ApplicationCore.DTOs.Requests.Account.Password;
using ClassUP.ApplicationCore.DTOs.Requests.Account_Management;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Refresh;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Login;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("iplimit")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IUserTokenService _userTokenService;

        public AccountController(
            IAuthService authservice,
            UserManager<AppUser> userManager,
            IUserTokenService userTokenService)
        {
            _authservice = authservice;
            _userTokenService = userTokenService;
        }

        #region Register & Login & Logout

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var user = await _authservice.RegisterAsync(dto);
            return Ok(user);
        }

        /// <summary>
        /// Logs in user and returns JWT + refresh token.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authservice.LoginAsync(dto);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Ok(result);
        }

        /// <summary>
        /// Logs out user and revokes all refresh tokens.
        /// </summary>
        [Authorize]
        [HttpDelete("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        #region Token Endpoints

        /// <summary>
        /// Refreshes JWT using refresh token cookie.
        /// </summary>
        [HttpGet("refresh-token")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshtoken = Request.Cookies["refreshtoken"];

            var result = await _userTokenService.RefreshTokenAsync(refreshtoken);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        /// <summary>
        /// Revokes a refresh token.
        /// </summary>
        [HttpPost("revoke-token")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        #endregion

        #region Forgot Password

        /// <summary>
        /// Sends password reset code to email.
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            await _authservice.SendResetPasswordCode(dto.Email);
            return NoContent();
        }

        /// <summary>
        /// Resets password using reset code.
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            await _authservice.ResetPasswordAsync(dto);
            return NoContent();
        }

        #endregion

        #region Helpers

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
    }
}