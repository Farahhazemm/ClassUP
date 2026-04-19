using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Account_Management;
using ClassUP.ApplicationCore.DTOs.Responses.User;
using ClassUP.ApplicationCore.Services.Account_Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Manages authenticated user profile operations (Get, Update, Image, Password).
    /// </summary>
    [Route("Me")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("userlimit")]
    public class Account_ManagementController : ControllerBase
    {
        private readonly IAccountManagementService _accountManagementService;

        public Account_ManagementController(IAccountManagementService accountManagementService)
        {
            _accountManagementService = accountManagementService;
        }

        #region Get Profile

        /// <summary>
        /// Retrieves the current authenticated user's profile information.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Info()
        {
            var result = await _accountManagementService.GetProfileAsync(User.GetUserId()!);
            return Ok(result);
        }

        #endregion

        #region Update Profile

        /// <summary>
        /// Updates user profile information (name, bio, phone).
        /// </summary>
        [HttpPut("Info")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            await _accountManagementService.UpdateProfileAsync(User.GetUserId()!, dto);
            return NoContent();
        }

        #endregion

        #region Update Profile Image

        /// <summary>
        /// Updates the user's profile image.
        /// </summary>
        [HttpPut("Profile-Image")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageDTO dto)
        {
            var userId = User.GetUserId()!;
            await _accountManagementService.UpdateProfileImageAsync(userId, dto.Image);

            return Created();
        }

        #endregion

        #region Change Password

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        [HttpPut("Change-Password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            await _accountManagementService.ChangePasswordAsync(User.GetUserId()!, dto);
            return NoContent();
        }

        #endregion
    }
}