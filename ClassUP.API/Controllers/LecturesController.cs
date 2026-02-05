using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.ApplicationCore.Services.Lectures;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        public readonly IVideoService _videoService;
        public LecturesController(ILectureService lectureService, IVideoService videoService)
        {
            _lectureService = lectureService;
            _videoService = videoService;

        }
        #region GetAll
        [HttpGet("GetAllLectures")]
        public async Task<IActionResult> GetAllLectures([FromQuery] FilterOptions filter)
        {
            var Courses = await _lectureService.GetLecturesAsync(filter);
            return Ok(Courses);
        }

        #endregion


        #region GetBYId
        [HttpGet("{lectureId}")]
        public async Task<IActionResult> GetById(int lectureId)
        {
            var lecture = await _lectureService.GetByIdAsync(lectureId);
            return Ok(lecture);

        }

        #endregion


        #region GetSectionLectures
        [HttpGet("section/{sectionId}/lectures")]
        public async Task<IActionResult> GetLecturesBySection(int sectionId)
        {
            var lectures = await _lectureService.GetBySectionIdAsync(sectionId);
            return Ok(lectures);
        }

        #endregion

        #region Create
        [HttpPost("section/lectures")]
        public async Task<IActionResult> Create([FromBody] CreateLectureRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lecture = await _lectureService.AddAsync(request);

            return CreatedAtAction("GetById", new { lectureId = lecture.Id }, lecture);
        }
        #endregion

        #region Update

        [HttpPatch("lectures/{id}")]
        public async Task<IActionResult> UpdateLecture(int id, [FromBody] UpdateLectureRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

         await _lectureService.UpdateAsync(id, request);


            return NoContent();
        }
        #endregion

        #region Delete
        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> DeleteLecture(int lectureId)
        {
            await _lectureService.DeleteAsync(lectureId);
            return NoContent();
        }


        #endregion


        #region UploadVideo
        [HttpPost("{lectureId}/video")]
        public async Task<IActionResult> UploadVideo(int lectureId, IFormFile file)
        {
            await _lectureService.UploadLectureVideoAsync(lectureId, file);
            return NoContent();
        }

        #endregion

        #region DeleteVideo
        [HttpDelete("lecture/video/{lectureId}")]
        public async Task<IActionResult> DeleteVideo(int lectureId)
        {

            await _lectureService.DeleteLectureVideoAsync(lectureId);

            return NoContent();
        } 
        #endregion



    }
}
