using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.ApplicationCore.Services.Lectures;
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
        [HttpGet("GetAllLectures")]
        public async Task<IActionResult> GetAllLectures([FromQuery] FilterOptions filter)
        {
            var Courses = await _lectureService.GetLecturesAsync(filter);
            return Ok(Courses);
        }

        [HttpGet("{LectureId}")]
        public async Task<IActionResult> GetByIdAsync(int LectureId)
        {
            var lecture = await _lectureService.GetByIdAsync(LectureId);
            return Ok(lecture);

        }

        }
    }
