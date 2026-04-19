using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using ClassUP.ApplicationCore.DTOs.Responses.Reviews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.Reviws
{
    public interface IReviewService
    {
        // Pass userId from Claims instead of body/query
        Task AddAsync(CourseReviewDTO request, string userId);
        Task UpdateAsync(UpdateReviewDTO request, string userId);
        Task DeleteAsync(int reviewId, string userId);

        Task<List<CourseReviewResponseDTO>> GetAllAsync(int courseId);
    }
}
