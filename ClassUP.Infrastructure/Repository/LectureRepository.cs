using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses;
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
    public class LectureRepository : BaseRepository<Lecture>, ILectureRepository
    {
        private AppDbContext _dbSet;
        public LectureRepository(AppDbContext dbSet) : base(dbSet)
        {
            _dbSet = dbSet;
        }

        public async Task<Lecture?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet.Lectures
           .Include(l => l.VideoContent)
           .Include(l => l.ArticleContent)
           .Include(l => l.LectureProgresses)
           .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
