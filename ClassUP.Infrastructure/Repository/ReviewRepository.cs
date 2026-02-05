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
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly AppDbContext _db;
        public ReviewRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Review>> GetByCourseIdAsync(int courseId)
        {
            return await _db.Reviews
                .AsNoTracking()
                .Include(r => r.User)
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
