using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Services.LectursProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles lecture progress tracking (complete, uncomplete, check, and analytics).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LectureProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public LectureProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        #region Complete Lecture

        /// <summary>
        /// Marks a lecture as completed for the current user.
        /// </summary>
        [HttpPost("complete-lecture/{lectureId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkASCompleted(int lectureId)
        {
            var userId = User.GetUserId();

            await _progressService.MarkLessonAsCompletedAsync(lectureId, userId);

            return Ok(new { message = "Lecture marked as completed" });
        }

        #endregion

        #region Uncomplete Lecture

        /// <summary>
        /// Marks a lecture as not completed.
        /// </summary>
        [HttpDelete("uncomplete-lecture/{lectureId}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnCompleteLesson(int lectureId)
        {
            var userId = User.GetUserId();

            await _progressService.UnCompleteLessonAsync(lectureId, userId);

            return Ok("Lecture uncompleted successfully.");
        }

        #endregion

        #region Check Lecture Completion

        /// <summary>
        /// Checks if a lecture is completed by the current user.
        /// </summary>
        [HttpGet("is-lecture-completed/{lectureId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsLessonCompleted(int lectureId)
        {
            var userId = User.GetUserId();

            var isCompleted = await _progressService.IsLessonCompletedAsync(lectureId, userId);

            return Ok(new { isCompleted });
        }

        #endregion

        #region Get Completed Lectures

        /// <summary>
        /// Retrieves all completed lectures for a specific course.
        /// </summary>
        [HttpGet("completed-lecture/{courseId}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompletedLessons(int courseId)
        {
            var userId = User.GetUserId();

            var completedLessonIds = await _progressService.GetCompletedLessonsAsync(courseId, userId);

            return Ok(completedLessonIds);
        }

        #endregion

        #region Recalculate Progress

        /// <summary>
        /// Recalculates total course progress for an enrollment.
        /// </summary>
        [HttpPost("recalculate-progress/{enrollmentId}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RecalculateProgress(int enrollmentId)
        {
            var progress = await _progressService.RecalculateProgressAsync(enrollmentId);

            return Ok(new { progress });
        }

        #endregion
    }
}