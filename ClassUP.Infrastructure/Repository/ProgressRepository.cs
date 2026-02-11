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
    public class ProgressRepository : BaseRepository<LectureProgress>, IProgressRepository
    {
        private readonly AppDbContext _db;
        public ProgressRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<LectureProgress> GetByEnrollmentAndLectureAsync(int enrollmentId, int lectureId)
        {
            return await _db.LectureProgresses
                .FirstOrDefaultAsync(lp =>
                    lp.EnrollmentId == enrollmentId &&
                    lp.LectureId == lectureId);
        }
    }
}
