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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
       
        public UnitOfWork(  AppDbContext db)
        {
            _db = db;
            Courses = new CourseRepository(_db);
        }
        public ICourseRepository Courses { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }


        public void Dispose()
        {
            _db.Dispose();

        }
    }
}
