using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using ClassUP.ApplicationCore.DTOs.Responses.Reviews;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Reviws
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(CourseReviewDTO reviewDTO)
        {

            var review = new Review
            {
                CourseId = reviewDTO.CourseId,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,

               UserId = reviewDTO.StudentId   //Need User By Claim
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CourseReviewResponseDTO>> GetAllAsync(int courseId)
        {
            //  safe check  >> remember Do it at  User  in Add review
            if (!await _unitOfWork.Courses.ExistsAsync(c => c.Id == courseId))
                throw new KeyNotFoundException("Course not found");

            var reviews = await _unitOfWork.Reviews.GetByCourseIdAsync(courseId);

            return reviews.Select(r => new CourseReviewResponseDTO
            {
                ReviewId = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,

                UserId = r.UserId,
                UserFullName = $"{r.User.FirstName} {r.User.LastName}"
            }).ToList();
        }
        public async Task UpdateAsync(UpdateReviewDTO reviewDTO)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewDTO.ReviewId);

            if (review == null)
                throw new KeyNotFoundException("Review not found");

            // Authorization check
            if (review.UserId != reviewDTO.UserId)
                throw new UnauthorizedAccessException("You cannot update this review");

            // Partial Update
            if (reviewDTO.Rating.HasValue)
                review.Rating = reviewDTO.Rating.Value;

            if (!string.IsNullOrWhiteSpace(reviewDTO.Comment))
                review.Comment = reviewDTO.Comment;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int reviewId, int userId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);

            if (review == null)
                throw new KeyNotFoundException("Review not found");

            // Authorization
            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You are not allowed to delete this review");

            await _unitOfWork.Reviews.DeleteAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }



    }
}
