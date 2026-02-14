
using ClassUP.ApplicationCore.DTOs.Requests.ForgotPassword;

using ClassUP.ApplicationCore.DTOs.Responses.ForgotPassword;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.ApplicationCore.Services.IIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [ApiController]
    [Route("api/forgot-password")]
    [Tags("Forgot Password")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPasswordService _forgotPasswordService;

        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpPost("request")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> RequestAsync([FromBody] ForgotPasswordRequestDTO dto)
        {
            await _forgotPasswordService.RequestAsync(dto);
            return Accepted();
        }

      
        [HttpPut("verify")]
        [ProducesResponseType(typeof(VerifyForgotPasswordResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyAsync([FromBody] VerifyForgotPasswordDTO dto)
        {
            var token = await _forgotPasswordService.VerifyAsync(dto);
            return Ok(token);
        }

        
        [HttpPut("reset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ResetAsync([FromBody] ResetPasswordDTO dto)
        {
            await _forgotPasswordService.ResetAsync(dto);
            return NoContent();
        }
    }
}
