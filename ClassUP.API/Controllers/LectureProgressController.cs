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

        [Authorize]
        [HttpDelete("uncomplete-lesson/{lectureId}")]
        public async Task<IActionResult> UnCompleteLesson(int lectureId)
        {
            var userId = User.GetUserId(); 
            await _progressService.UnCompleteLessonAsync(lectureId, userId);
            return Ok("Lesson uncompleted successfully.");
        }

        [Authorize]
        [HttpGet("is-lesson-completed/{lectureId}")]
        public async Task<IActionResult> IsLessonCompleted(int lectureId)
        {
            var userId = User.GetUserId(); 
            var isCompleted = await _progressService.IsLessonCompletedAsync(lectureId, userId);
            return Ok(new { isCompleted });
        }

        [Authorize]
        [HttpGet("completed-lessons/{courseId}")]
        public async Task<IActionResult> GetCompletedLessons(int courseId)
        {
            var userId = User.GetUserId(); 
            var completedLessonIds = await _progressService.GetCompletedLessonsAsync(courseId, userId);
            return Ok(completedLessonIds);
        }




    }


}
