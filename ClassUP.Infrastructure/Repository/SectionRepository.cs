using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ClassUP.Infrastructure.Repository
{
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        private readonly AppDbContext _db;
        public SectionRepository(AppDbContext db) : base(db)
        {
            _db = db;   
        }

        public async Task<IEnumerable<Section>> GetByCourseIdAsync(int id)
        {
            return await _db.Sections
         .Where(s => s.CourseId == id)
         .OrderBy(s => s.OrderIndex)
         .ToListAsync();
        }
    }
}
