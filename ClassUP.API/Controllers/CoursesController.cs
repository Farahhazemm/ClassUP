using ClassUP.API.Requests;
using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Cources;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController( ICourseService courseService)
        {
            _courseService = courseService;
        }
        #region Read Actions
        // use in test

        // [Authorize(Roles ="Admin")]
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses([FromQuery] FilterOptions filter)
        {
            var Courses = await _courseService.GetAllCourses(filter);
            return Ok(Courses);
        }
        [HttpGet("GetInstructorCourses/{instructorId}")]
        public async Task<IActionResult> GetInstructorCoursesAsync(int instructorId, [FromQuery] FilterOptions filter)
        {
            var courses = await _courseService.GetInstructorCoursesAsync(instructorId, filter);

            if (!courses.Any())
                return NoContent();

            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var Course = await _courseService.GetByIdAsync(courseId);
            if (Course == null)
                return NotFound($"Course not found, {courseId}");
            return Ok(Course);
        }
        #endregion

        [HttpPost]
        //userId passed from query until Auth is implemented
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseRequest request, [FromQuery]  int UserId)
        {
            //Enum Check
            if (!Enum.TryParse<CourseLevel>(request.Level, true, out var level))
            {
                return BadRequest("Invalid course level");
            }
            // map req To DTO
            var courseDTO = new CreateCourseDTO
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Level = level,
                Language = request.Language,
                IsActive = request.IsActive
            };
            // Map Thumb to DTO
            var thumbnailDTO = new ThumbnailDTO
            {
                FileStream = request.Thumbnail.OpenReadStream(),
                MimeType = request.Thumbnail.ContentType,
                FileSize = request.Thumbnail.Length
            };

            var course = await _courseService.CreateCourse(courseDTO, thumbnailDTO, UserId);


            return CreatedAtAction(
        "GetCourseById",
        new { courseId = course.Id},course);
        }

    }
}
