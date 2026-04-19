using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using ClassUP.ApplicationCore.Services.Reviws;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles course reviews operations (add, update, delete, and retrieve reviews).
    /// </summary>
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

        /// <summary>
        /// Adds a new review for a course.
        /// </summary>
        /// <response code="204">Review added successfully.</response>
        /// <response code="400">Invalid request or user not allowed.</response>
        /// <response code="401">Unauthorized.</response>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("add-course-review")]
        [EnableRateLimiting("userlimit")]
        public async Task<IActionResult> AddReview([FromBody] CourseReviewDTO reviewDTO)
        {
            var userId = User.GetUserId();
            await _reviewService.AddAsync(reviewDTO, userId);
            return NoContent();
        }

        #endregion

        #region Get Reviews

        /// <summary>
        /// Retrieves all reviews for a specific course.
        /// </summary>
        /// <response code="200">Returns list of course reviews.</response>
        /// <response code="404">Course not found.</response>
        [HttpGet("get-course-review/{courseId}")]
        public async Task<IActionResult> GetAllReviews(int courseId)
        {
            var reviews = await _reviewService.GetAllAsync(courseId);
            return Ok(reviews);
        }

        #endregion

        #region Update Review

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <response code="204">Review updated successfully.</response>
        /// <response code="400">Invalid data or unauthorized update.</response>
        /// <response code="401">Unauthorized.</response>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPatch("update-course-review")]
        [EnableRateLimiting("userlimit")]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewDTO reviewDTO)
        {
            var userId = User.GetUserId();
            await _reviewService.UpdateAsync(reviewDTO, userId);
            return NoContent();
        }

        #endregion

        #region Delete Review

        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        /// <response code="204">Review deleted successfully.</response>
        /// <response code="404">Review not found.</response>
        /// <response code="401">Unauthorized.</response>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("delete-course-review/{reviewId}")]
        [EnableRateLimiting("userlimit")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var userId = User.GetUserId();
            await _reviewService.DeleteAsync(reviewId, userId);
            return NoContent();
        }

        #endregion
    }
}
