using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.ApplicationCore.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public EmailVerificationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO request)
        {
           
         
            await _authService.ConfirmEmailAsync(request);
            return Ok(new { success = true, message = "Email confirmed successfully." });
        }


    }
}
