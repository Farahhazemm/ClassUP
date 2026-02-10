using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.DTOs.Responses.Sections;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.Sections
{
    public class SectionService : ISectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Create
        public async Task<SectionDTO> CreateAsync(int courseId, CreateSectionRequest request, string userId, bool isAdmin)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new ArgumentException("Course not found");

            // Authorization check: only course owner or admin can create
            if (course.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Not authorized to add section to this course");

            var section = new Section
            {
                CourseId = courseId,
                Title = request.Title,
                OrderIndex = request.OrderIndex
            };

            await _unitOfWork.Sections.AddAsync(section);
            await _unitOfWork.SaveChangesAsync();

            return new SectionDTO
            {
                Id = section.Id,
                Title = section.Title,
                OrderIndex = section.OrderIndex,
                CourseId = section.CourseId
            };
        }
        #endregion

        #region Update
        public async Task UpdateAsync(int id, UpdateSectionRequest request, string userId, bool isAdmin)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(id);
            if (section == null)
                throw new KeyNotFoundException($"Section with id {id} not found");

            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId);
            if (course == null)
                throw new ArgumentException("Course not found");

            // Authorization check
            if (course.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Not authorized to update this section");

            if (!string.IsNullOrWhiteSpace(request.Title))
                section.Title = request.Title.Trim();

            if (request.OrderIndex.HasValue)
                section.OrderIndex = request.OrderIndex.Value;

            _unitOfWork.Sections.UpdateAsync(section);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(int id, string userId, bool isAdmin)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(id);
            if (section == null)
                throw new KeyNotFoundException($"Section with id {id} not found");

            var course = await _unitOfWork.Courses.GetByIdAsync(section.CourseId);
            if (course == null)
                throw new ArgumentException("Course not found");

            // Authorization check
            if (course.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Not authorized to delete this section");

            _unitOfWork.Sections.DeleteAsync(section);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region GetById
        public async Task<SectionDTO> GetByIdAsync(int id)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(id);
            if (section == null)
                throw new KeyNotFoundException($"Section with id {id} not found");

            return new SectionDTO
            {
                Id = section.Id,
                Title = section.Title,
                OrderIndex = section.OrderIndex,
                CourseId = section.CourseId
            };
        }
        #endregion

        #region GetCourseSections
        public async Task<IEnumerable<SectionDTO>> GetCourseSectionsAsync(int courseId)
        {
            var sections = await _unitOfWork.Sections.GetByCourseIdAsync(courseId);
            if (sections == null || !sections.Any())
                return Enumerable.Empty<SectionDTO>();

            return sections
                .OrderBy(s => s.OrderIndex)
                .Select(s => new SectionDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    OrderIndex = s.OrderIndex,
                    CourseId = s.CourseId
                })
                .ToList();
        }
        #endregion
    }
}
