using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.ApplicationCore.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles email verification operations (confirm email & resend confirmation).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("iplimit")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public EmailVerificationController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Confirm Email

        /// <summary>
        /// Confirms user email using verification code.
        /// </summary>
        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO request)
        {
            await _authService.ConfirmEmailAsync(request);

            return Ok(new
            {
                success = true,
                message = "Email confirmed successfully."
            });
        }

        #endregion

        #region Resend Confirmation Email

        /// <summary>
        /// Resends email confirmation code to user email.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("resend-confirmation-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailDTO request)
        {
            await _authService.ResendConfirmationEmailAsync(request);

            return Ok(new
            {
                success = true,
                message = "A confirmation code has been resent."
            });
        }

        #endregion
    }
}