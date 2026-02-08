using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly AppDbContext _db;
        public CourseRepository(AppDbContext db)  :base(db) 
        {
            _db = db;
        }
        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(string instructorId, FilterOptions filter)
        {
            var query = _db.Courses.Where(q => q.UserId == instructorId);

            if (filter != null && filter.PageNumber > 0 && filter.PageSize > 0)
            {
                query = query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            return await query.ToListAsync();

        }

        public async Task<IEnumerable<Course>> GetCategoryCoursesAsync(int categoryId)
        {
            return await _db.Courses
         .Where(c => c.CategoryId == categoryId)
         .AsNoTracking()
         .ToListAsync();
        }

    }
}


