using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authservice;

        public AccountController(IAuthService authservice)
        {
            _authservice = authservice;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var user = await _authservice.RegisterAsync(dto);
            return Ok(user);
        }
    }
}
