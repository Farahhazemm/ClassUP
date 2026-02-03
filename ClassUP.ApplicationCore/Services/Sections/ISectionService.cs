using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.DTOs.Responses.Sections;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace ClassUP.ApplicationCore.Services.Sections
{
    public interface ISectionService
    {
        Task<SectionDTO> CreateAsync(int courseId, CreateSectionRequest request);
        Task UpdateAsync(int id, UpdateSectionRequest request);
        Task DeleteAsync(int id);
        Task<SectionDTO> GetByIdAsync(int id);
        Task<IEnumerable<SectionDTO>> GetCourseSectionsAsync(int id);
    }
}
