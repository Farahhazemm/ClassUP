using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using ClassUP.ApplicationCore.DTOs.Responses.Reviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Reviws
{
    public interface IReviewService
    {
        Task AddAsync(CourseReviewDTO request);
        Task UpdateAsync(UpdateReviewDTO request);
        Task<List<CourseReviewResponseDTO>> GetAllAsync(int courseId);
    }
}
