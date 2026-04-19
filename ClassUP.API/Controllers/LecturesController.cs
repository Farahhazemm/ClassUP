using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.ApplicationCore.Services.Lectures;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles lecture management operations (CRUD, section lectures, video upload).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        #region Get All Lectures

        /// <summary>
        /// Retrieves all lectures with pagination and filters.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("GetAllLectures")]
        [ProducesResponseType(typeof(PaginatedList<LectureDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLectures([FromQuery] FilterOptions filter)
        {
            var lectures = await _lectureService.GetLecturesAsync(filter);
            return Ok(lectures);
        }

        #endregion

        #region Get Lecture By Id

        /// <summary>
        /// Retrieves a lecture by its ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{lectureId}")]
        [ProducesResponseType(typeof(LectureDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int lectureId)
        {
            var lecture = await _lectureService.GetByIdAsync(lectureId);
            return Ok(lecture);
        }

        #endregion

        #region Get Section Lectures

        /// <summary>
        /// Retrieves all lectures under a specific section.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("section/{sectionId}/lectures")]
        [ProducesResponseType(typeof(IEnumerable<LectureDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLecturesBySection(int sectionId)
        {
            var lectures = await _lectureService.GetBySectionIdAsync(sectionId);
            return Ok(lectures);
        }

        #endregion

        #region Create Lecture

        /// <summary>
        /// Creates a new lecture in a section.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("section/lectures")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(typeof(LectureDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateLectureRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            var lecture = await _lectureService.AddAsync(request, userId, isAdmin);

            return CreatedAtAction(
                nameof(GetById),
                new { lectureId = lecture.Id },
                lecture
            );
        }

        #endregion

        #region Update Lecture

        /// <summary>
        /// Updates an existing lecture.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPatch("lectures/{lectureId}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] UpdateLectureRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.UpdateAsync(lectureId, request, userId, isAdmin);

            return NoContent();
        }

        #endregion

        #region Delete Lecture

        /// <summary>
        /// Deletes a lecture.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("{lectureId}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLecture(int lectureId)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.DeleteAsync(lectureId, userId, isAdmin);

            return NoContent();
        }

        #endregion

        #region Upload Video

        /// <summary>
        /// Uploads a video for a lecture.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("{lectureId}/video")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadVideo(int lectureId, IFormFile file)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.UploadLectureVideoAsync(lectureId, file, userId, isAdmin);

            return NoContent();
        }

        #endregion

        #region Delete Video

        /// <summary>
        /// Deletes lecture video.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("lecture/video/{lectureId}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVideo(int lectureId)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.DeleteLectureVideoAsync(lectureId, userId, isAdmin);

            return NoContent();
        }

        #endregion
    }
}