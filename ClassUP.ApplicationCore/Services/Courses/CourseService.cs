using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Cources;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Course> GetByIdAsync(int id)
        {
           return await _unitOfWork.Courses.GetByIdAsync(id);
        }

       

        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId, FilterOptions filter)
        {
            return await _unitOfWork.Courses.GetInstructorCoursesAsync(instructorId, filter);
        }
        
    }
}
