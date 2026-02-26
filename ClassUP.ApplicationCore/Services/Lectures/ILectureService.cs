using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.ApplicationCore.Helpers.Filters;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.Lectures
{
    public interface ILectureService
    {

        Task<PaginatedList<LectureDTO>> GetLecturesAsync(FilterOptions filterOptions);

    
        Task<LectureDetailDto?> GetByIdAsync(int id);

     
        Task<IEnumerable<LectureDTO>> GetBySectionIdAsync(int sectionId);

        
        Task<LectureDTO> AddAsync(CreateLectureRequest request, string userId, bool isAdmin);

     
        Task UpdateAsync(int lectureId, UpdateLectureRequest request, string userId, bool isAdmin);

      
        Task DeleteAsync(int lectureId, string userId, bool isAdmin);

      
        Task UploadLectureVideoAsync(int lectureId, IFormFile file, string userId, bool isAdmin);

    
        Task DeleteLectureVideoAsync(int lectureId, string userId, bool isAdmin);
    }
}
