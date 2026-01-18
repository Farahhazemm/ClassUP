using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.Services.Courses;
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
        // use in test

       // [Authorize(Roles ="Admin")]
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult>GetAllCourses([FromQuery] FilterOptions filter)
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
                return NotFound($"Course not found, {courseId }");
            return Ok(Course);
        }

    }
}
