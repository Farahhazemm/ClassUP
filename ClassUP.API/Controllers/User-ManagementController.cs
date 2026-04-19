using ClassUP.ApplicationCore.DTOs.Requests.User;
using ClassUP.ApplicationCore.Services.User_Management;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.Admin)]
    public class User_ManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;

        public User_ManagementController(IUserManagementService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users (Admin only).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by id (Admin only).
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _userService.GetUserAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Create a new user (Admin only).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var user = await _userService.CreateUserAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Update user information (Admin only).
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDTO dto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, dto);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Toggle user account status (activate / deactivate).
        /// </summary>
        [HttpPatch("{id}/account-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAccountStatus(string id)
        {
            var result = await _userService.ToggleAsync(id);
            return Ok(result);
        }
    }
}