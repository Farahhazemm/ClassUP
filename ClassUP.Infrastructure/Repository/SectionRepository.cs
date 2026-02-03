using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Repository
{
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(AppDbContext db) : base(db)
        {
        }
    }
}
