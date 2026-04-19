using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Courses;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles course operations (CRUD, filtering, instructor courses, categories).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Read

        /// <summary>
        /// Retrieves all courses with pagination and filters.
        /// </summary>
        [HttpGet("GetAllCourses")]
        [ProducesResponseType(typeof(PaginatedList<AllCoursesDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses([FromQuery] FilterOptions filter)
        {
            var Courses = await _courseService.GetAllCourses(filter);
            return Ok(Courses);
        }

        /// <summary>
        /// Retrieves courses for the logged-in instructor.
        /// </summary>
        [Authorize]
        [HttpGet("my-courses")]
        [ProducesResponseType(typeof(IEnumerable<AllCoursesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetInstructorCoursesAsync([FromQuery] FilterOptions filter)
        {
            var userId = User.GetUserId();
            var courses = await _courseService.GetInstructorCoursesAsync(userId, filter);

            if (!courses.Any())
                return NoContent();

            return Ok(courses);
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{courseId}")]
        [ProducesResponseType(typeof(CourseDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var Course = await _courseService.GetByIdAsync(courseId);
            return Ok(Course);
        }

        /// <summary>
        /// Retrieves all courses under a specific category.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("/Category/{categoryId}/Courses")]
        [ProducesResponseType(typeof(IEnumerable<AllCoursesDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCoursesByCategory(int categoryId)
        {
            var courses = await _courseService.GetCategoryCourses(categoryId);
            return Ok(courses);
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new course (Instructor/Admin only).
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateCourseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableRateLimiting("userlimit")]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseRequest request)
        {
            var userId = User.GetUserId();

            var course = await _courseService.CreateCourse(request, userId);

            return CreatedAtAction(
                nameof(GetCourseById),
                new { courseId = course.Id },
                course
            );
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPatch("{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EnableRateLimiting("userlimit")]
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

        /// <summary>
        /// Deletes a course.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EnableRateLimiting("userlimit")]
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