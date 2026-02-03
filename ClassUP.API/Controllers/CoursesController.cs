using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ClassUP.ApplicationCore.DTOs.Requests.Courses;

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
            
            return Ok(Course);
        }
        #endregion

        #region Create
        [HttpPost]
        //userId passed from query until Auth is implemented
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseRequest request, [FromQuery] int UserId)
        {


            var course = await _courseService.CreateCourse(request, UserId);

            return CreatedAtAction("GetCourseById", new { courseId = course.Id }, course);
        }
        #endregion


        #region Update
        [HttpPatch("{courseId}")]
        public async Task<IActionResult> UpdateCourse([FromForm] UpdateCourseRequest request, [FromQuery] int userId, [FromRoute] int courseId)
        {
            request.courseId = courseId;
            await _courseService.UpdateCourse(userId, request);
            return NoContent();
        }

        #endregion

        #region Delete
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int courseId)
        {
            await _courseService.DeleteCourse(courseId);
            return NoContent();
        } 
        #endregion

    }
}
