using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Services.LectursProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LectureProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;
        public LectureProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }
        [Authorize]
        [HttpPost("complete-lecture/{lectureId}")]
        public async Task<IActionResult> MarkASCompleted(int lectureId)
        {
            var userId = User.GetUserId(); 
            await _progressService.MarkLessonAsCompletedAsync(lectureId, userId);
            return Ok(new { message = "Lesson marked as completed" });
        }

    }

  
}
