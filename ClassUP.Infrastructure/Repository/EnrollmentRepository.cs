using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.Helpers.Filters;
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
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly AppDbContext _db;
        public EnrollmentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Enrollment?> GetEnrollmentAsync(string userId, int courseId)
        {
            return await _db.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    e.UserId == userId &&
                    e.CourseId == courseId);
        }

        public async Task<PaginatedList<Enrollment>> GetStudentEnrollmentsAsync(string userId, FilterOptions filter)
        {
            var query = _db.Enrollments.Where(e => e.UserId == userId);

            var count = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedList<Enrollment>(items, count, filter.PageNumber, filter.PageSize);
        }

        public async Task<bool> IsEnrolledAsync(string
            userId, int courseId)
        {
            return await _db.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
        }
    }
}
