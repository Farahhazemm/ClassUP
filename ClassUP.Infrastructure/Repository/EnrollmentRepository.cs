using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Repository
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext db) : base(db)
        {
        }
    }
}
