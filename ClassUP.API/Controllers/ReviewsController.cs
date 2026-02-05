using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using ClassUP.ApplicationCore.Services.Reviws;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
           
            _reviewService = reviewService; 

        }

        [HttpPost("add-course-review")]
        public async Task<IActionResult> AddRating([FromBody] CourseReviewDTO reviewDTO)
        {
            await  _reviewService.AddAsync(reviewDTO);
            return NoContent();
        }

        [HttpGet("get-course-review/{courseId}")]
        public async Task<IActionResult> GetAllReviews(int courseId)
        {
            var reviews = await _reviewService.GetAllAsync(courseId);
            return Ok(reviews);
        }
    }
}
