using ClassUP.ApplicationCore.Common.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using ClassUP.Domain.Models;
using ClassUP.ApplicationCore.DTOs.Cources;
namespace ClassUP.ApplicationCore.Services.Courses

{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId,FilterOptions filter );
        Task<Course> GetByIdAsync(int id);
    }
}
