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

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent(CreateEnrollmentRequest request)
        {
            var enroll = await _enrollmentService.CreateAsync(request);
            return Ok(enroll);
        }
    }
}
