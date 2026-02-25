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
    }
}
