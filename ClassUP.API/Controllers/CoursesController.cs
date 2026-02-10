using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Courses;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.Domain.Constants;
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

        [Authorize]
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses([FromQuery] FilterOptions filter)
        {
            var Courses = await _courseService.GetAllCourses(filter);
            return Ok(Courses);
        }


        [Authorize]
        [HttpGet("my-courses")]
        public async Task<IActionResult> GetInstructorCoursesAsync( [FromQuery] FilterOptions filter)
        {
            var userId = User.GetUserId();

            var courses = await _courseService.GetInstructorCoursesAsync(userId, filter);

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

        [HttpGet("/Category/{categoryId}/Courses")]
        public async Task<ActionResult> GetCoursesByCategory(int categoryId)
        {
            var courses = await _courseService.GetCategoryCourses(categoryId);
            return Ok(courses);
        }
        #endregion

        #region Create
        [Authorize]
        
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseRequest request)
        {
            var userId = User.GetUserId();

            var course = await _courseService.CreateCourse(request, userId);

            return CreatedAtAction(
                "GetCourseById",
                new { courseId = course.Id },
                course
            );
        }
        #endregion


        #region Update
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPatch("{courseId}")]
        public async Task<IActionResult> UpdateCourse([FromForm] UpdateCourseRequest request, [FromRoute] int courseId)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            request.courseId = courseId;

            await _courseService.UpdateCourse(userId, isAdmin, request);
            return NoContent();
        }

        #endregion

        #region Delete
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int courseId)
        {
            var userId = User.GetUserId(); 
            var isAdmin = User.IsInRole(AppRoles.Admin); 

            await _courseService.DeleteCourse(courseId, userId, isAdmin);

            return NoContent();
        }



        #endregion

    }
}
