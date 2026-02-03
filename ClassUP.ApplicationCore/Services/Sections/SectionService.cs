using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.DTOs.Responses.Sections;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Sections
{
    public class SectionService : ISectionService
    {
        private readonly IUnitOfWork  _unitOfWork;
        public SectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        #region Create
        public async Task<SectionDTO> CreateAsync(int courseId, CreateSectionRequest request)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new ArgumentException("Course not found");


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

            };
        }

       
        #endregion

        #region Update
        public async Task UpdateAsync(int id, UpdateSectionRequest request)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(id);
            if (section == null)
                throw new KeyNotFoundException($"Section with id {id} not found");
            if (request.Title != null)
                section.Title = request.Title.Trim();
            if (request.OrderIndex.HasValue)
                section.OrderIndex = request.OrderIndex.Value;
            _unitOfWork.Sections.UpdateAsync(section);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(int id)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(id);
            if (section == null)
                throw new KeyNotFoundException($"Section with id {id} not found");
            /* if (section.Lectures.Any())
                 throw new BusinessException("Cannot delete section that has lectures");*/
            _unitOfWork.Sections.DeleteAsync(section);
            await _unitOfWork.SaveChangesAsync();



        } 
        #endregion

    }
}
