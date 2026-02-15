using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
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
            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
            }
            return Ok(result);
        }
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshtoken = Request.Cookies["refreshtoken"];

            var result = await _userTokenService.RefreshTokenAsync(refreshtoken);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        private void SetRefreshTokenInCookie(string refreshToken,DateTime expires)
        {
            var cookieoptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),

            };
            Response.Cookies.Append("refreshtoken",refreshToken, cookieoptions);
        }

    }
}
