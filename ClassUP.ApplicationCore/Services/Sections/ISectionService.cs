using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.DTOs.Responses.Sections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.Sections
{
    public interface ISectionService
    {
        Task<IEnumerable<SectionDTO>> GetCourseSectionsAsync(int courseId);
        Task<SectionDTO> GetByIdAsync(int id);

        Task<SectionDTO> CreateAsync(int courseId, CreateSectionRequest request, string userId, bool isAdmin);
        Task UpdateAsync(int id, UpdateSectionRequest request, string userId, bool isAdmin);
        Task DeleteAsync(int id, string userId, bool isAdmin);
    }
}
