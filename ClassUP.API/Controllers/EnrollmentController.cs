using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Services.Enrollment;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
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
        #region Get All Enrollments (Admin)
        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllAsync();
            return Ok(enrollments);
        }
        #endregion

        #region Get Current Student Enrollments
        [HttpGet("get-student-enrollments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyEnrollments()
        {
            var userId = User.GetUserId(); // from JWT claims
            var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(userId);
            return Ok(enrollments);
        } 
        #endregion

        #region  Enroll in a Course

        [HttpPost("enroll/{courseId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EnrollStudent(int courseId)
        {
            var userId = User.GetUserId();
            var enroll = await _enrollmentService.CreateAsync(courseId,userId);
            return CreatedAtAction("GetById", new { id = enroll.EnrollmentId }, enroll);
        } 
        #endregion

        #region Get Enrollment By Id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            return Ok(enrollment);
        } 
        #endregion

        #region Check if Current User is Enrolled
        [HttpGet("check/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckEnrollment(int courseId)
        {
            var userId = User.GetUserId(); // from JWT claims
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
        [HttpDelete("unenroll/{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnEnroll(int courseId)
        {
            var userId = User.GetUserId(); // from JWT claims
            await _enrollmentService.UnEnrollAsync(courseId, userId);
            return NoContent();
        } 
        #endregion
    }
}
