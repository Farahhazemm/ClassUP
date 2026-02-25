using ClassUP.ApplicationCore.DTOs.Requests.User;
using ClassUP.ApplicationCore.Services.User_Management;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_ManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;
        public User_ManagementController( IUserManagementService userService)
        {
             _userService = userService;
        }

        // GET: /api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }


        // GET: /api/users/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _userService.GetUserAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var user = await _userService.CreateUserAsync(dto);

            return CreatedAtAction( nameof(GetById),  new { id = user.Id },  user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDTO dto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, dto);
            return Ok(updatedUser);
        }

    }
}
