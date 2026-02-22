using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Account_Management;
using ClassUP.ApplicationCore.Services.Account_Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("Me")]
    [ApiController]
    [Authorize]
    public class Account_ManagementController : ControllerBase
    {
        private readonly IAccountManagementService _accountManagementService;
        public Account_ManagementController( IAccountManagementService accountManagementService)
        {
            _accountManagementService = accountManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var result = await _accountManagementService.GetProfileAsync(User.GetUserId()!);
            return Ok(result);
        }

        [HttpPut("Info")]
        public async Task<IActionResult>UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            await _accountManagementService.UpdateProfileAsync(User.GetUserId()!,dto);
            return NoContent();
        }

    }
}
