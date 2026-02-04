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
        [HttpGet("GetAllLectures")]
        public async Task<IActionResult> GetAllLectures([FromQuery] FilterOptions filter)
        {
            var Courses = await _lectureService.GetLecturesAsync(filter);
            return Ok(Courses);
        }

        [HttpGet("{lectureId}")]
        public async Task<IActionResult> GetById(int lectureId)
        {
            var lecture = await _lectureService.GetByIdAsync(lectureId);
            return Ok(lecture);

        }

        [HttpPost("section/lectures")]
        public async Task<IActionResult>Create([FromBody] CreateLectureRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lecture = await _lectureService.AddAsync( request);

            return CreatedAtAction("GetById", new { lectureId = lecture.Id },lecture);
        }

        [HttpPost("lecture/video")]
        public async Task<IActionResult>UploadVideo( IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                return BadRequest("No video file provided");
            }

            await _videoService.UploadAsync(videoFile);

            return NoContent();
        }




    }
    }
