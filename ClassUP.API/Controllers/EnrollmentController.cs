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
            return CreatedAtAction("GetById", new {id = enroll.EnrollmentId},enroll);
        }
        [HttpGet("{id}")]
        public async Task <IActionResult>GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            return Ok(enrollment);  
        }
    }
}
