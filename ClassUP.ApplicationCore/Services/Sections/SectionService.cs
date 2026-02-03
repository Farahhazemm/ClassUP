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
                Id=section.Id,
                Title= section.Title,   
                OrderIndex = section.OrderIndex,

            };
        }
    }
}
