using ClassUP.ApplicationCore.DTOs.Requests.Enrollment;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.ApplicationCore.Services.Enrollment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseService _courseServices;
        public EnrollmentController(IEnrollmentService enrollmentService, ICourseService courseService)
        {
            _courseServices = courseService;
            _enrollmentService = enrollmentService;
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var enrollments = await _enrollmentService.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent(CreateEnrollmentRequest request)
        {
            var enroll = await _enrollmentService.CreateAsync(request);
            return CreatedAtAction("GetById", new {id = enroll.EnrollmentId},enroll);
        }
        [HttpGet("{id}")]
        public async Task <IActionResult>GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            return Ok(enrollment);  
        }
        [HttpGet("check/{courseId}")]
        public async Task<IActionResult> CheckEnrollment(int courseId,int userId)
        {
            // get user by claims neer
            var isEnrolled = await _enrollmentService.IsEnrolledAsync(courseId, userId);

            return Ok(new
            {
                CourseId = courseId,
                UserId = userId,
                IsEnrolled = isEnrolled
            });
        }

        [HttpDelete("unenroll/{courseId}")]
        public async Task<IActionResult> UnEnroll(int courseId, [FromQuery] int userId)
        {

            await _enrollmentService.UnEnrollAsync(courseId, userId);

            return NoContent();
        }

    }
}
