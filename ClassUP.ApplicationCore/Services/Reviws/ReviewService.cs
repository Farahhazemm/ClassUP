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
    }
}
