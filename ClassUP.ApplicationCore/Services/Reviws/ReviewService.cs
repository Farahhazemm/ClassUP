using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
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
    }
}
