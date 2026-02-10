using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.Services.Lectures;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        #region GetAll
        [AllowAnonymous]
        [HttpGet("GetAllLectures")]
        public async Task<IActionResult> GetAllLectures([FromQuery] FilterOptions filter)
        {
            var lectures = await _lectureService.GetLecturesAsync(filter);
            return Ok(lectures);
        }
        #endregion

        #region GetById
        [AllowAnonymous]
        [HttpGet("{lectureId}")]
        public async Task<IActionResult> GetById(int lectureId)
        {
            var lecture = await _lectureService.GetByIdAsync(lectureId);
            return Ok(lecture);
        }
        #endregion

        #region GetSectionLectures
        [AllowAnonymous]
        [HttpGet("section/{sectionId}/lectures")]
        public async Task<IActionResult> GetLecturesBySection(int sectionId)
        {
            var lectures = await _lectureService.GetBySectionIdAsync(sectionId);
            return Ok(lectures);
        }
        #endregion

        #region Create
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("section/lectures")]
        public async Task<IActionResult> Create([FromBody] CreateLectureRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);
            var lecture = await _lectureService.AddAsync(request, userId,isAdmin);
            return CreatedAtAction("GetById", new { lectureId = lecture.Id }, lecture);
        }
        #endregion

        #region Update
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPatch("lectures/{lectureId}")]
        public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] UpdateLectureRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.UpdateAsync(lectureId, request, userId, isAdmin);
            return NoContent();
        }
        #endregion

        #region Delete
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> DeleteLecture(int lectureId)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.DeleteAsync(lectureId, userId, isAdmin);
            return NoContent();
        }
        #endregion

        #region UploadVideo
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("{lectureId}/video")]
        public async Task<IActionResult> UploadVideo(int lectureId, IFormFile file)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _lectureService.UploadLectureVideoAsync(lectureId, file, userId, isAdmin);
            return NoContent();
        }
        #endregion

        #region DeleteVideo
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("lecture/video/{lectureId}")]
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
