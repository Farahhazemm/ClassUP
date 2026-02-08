using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        public Task<IEnumerable<Course>> GetInstructorCoursesAsync(string instructorId,FilterOptions filter);
        Task<IEnumerable<Course>> GetCategoryCoursesAsync(int categoryId);
    }
}
