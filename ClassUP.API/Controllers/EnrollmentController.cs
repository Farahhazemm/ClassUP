using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.ApplicationCore.Services.Enrollment;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles course enrollment operations (admin & student actions).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        #region Admin - Get All Enrollments

        /// <summary>
        /// Retrieves all enrollments (Admin only).
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<EnrollmentDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(FilterOptions filter)
        {
            var enrollments = await _enrollmentService.GetAllAsync(filter);
            return Ok(enrollments);
        }

        #endregion

        #region Student - My Enrollments

        /// <summary>
        /// Retrieves current logged-in student's enrollments.
        /// </summary>
        [HttpGet("get-student-enrollments")]
        [ProducesResponseType(typeof(PaginatedList<EnrollmentDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyEnrollments([FromQuery] FilterOptions filter)
        {
            var userId = User.GetUserId();

            var enrollments = await _enrollmentService
                .GetStudentEnrollmentsAsync(userId, filter);

            return Ok(enrollments);
        }

        #endregion

        #region Get Enrollment By Id

        /// <summary>
        /// Retrieves enrollment by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnrollmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            return Ok(enrollment);
        }

        #endregion

        #region Check Enrollment

        /// <summary>
        /// Checks if current user is enrolled in a course.
        /// </summary>
        [HttpGet("check/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckEnrollment(int courseId)
        {
            var userId = User.GetUserId();

            var isEnrolled = await _enrollmentService.IsEnrolledAsync(courseId, userId);

            return Ok(new
            {
                CourseId = courseId,
                UserId = userId,
                IsEnrolled = isEnrolled
            });
        }

        #endregion

        #region Unenroll

        /// <summary>
        /// Unenrolls current user from a course.
        /// </summary>
        [HttpDelete("unenroll/{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EnableRateLimiting("userlimit")]
        public async Task<IActionResult> UnEnroll(int courseId)
        {
            var userId = User.GetUserId();

            await _enrollmentService.UnEnrollAsync(courseId, userId);

            return NoContent();
        }

        #endregion
    }
}