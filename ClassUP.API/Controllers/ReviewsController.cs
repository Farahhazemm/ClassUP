using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using ClassUP.ApplicationCore.Services.Reviws;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
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

        #region Add Review
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("add-course-review")]
        public async Task<IActionResult> AddReview([FromBody] CourseReviewDTO reviewDTO)
        {
            var userId = User.GetUserId(); // From Claims
            await _reviewService.AddAsync(reviewDTO, userId);
            return NoContent();
        }
        #endregion

        #region Get Reviews
        [HttpGet("get-course-review/{courseId}")]
        public async Task<IActionResult> GetAllReviews(int courseId)
        {
            var reviews = await _reviewService.GetAllAsync(courseId);
            return Ok(reviews);
        }
        #endregion

        #region Update Review
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPatch("update-course-review")]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewDTO reviewDTO)
        {
            var userId = User.GetUserId(); // From Claims
            await _reviewService.UpdateAsync(reviewDTO, userId);
            return NoContent();
        }
        #endregion

        #region Delete Review
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("delete-course-review/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var userId = User.GetUserId(); // From Claims
            await _reviewService.DeleteAsync(reviewId, userId);
            return NoContent();
        }
        #endregion
    }
}
