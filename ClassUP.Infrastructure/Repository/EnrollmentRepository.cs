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

        public async Task<Enrollment> GetEnrollmentAsync(int userId, int courseId)
        {
            return await _db.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    e.UserId == userId &&
                    e.CourseId == courseId);
        }

        public async Task<bool> IsEnrolledAsync(int userId, int courseId)
        {
            return await _db.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
        }
    }
}
