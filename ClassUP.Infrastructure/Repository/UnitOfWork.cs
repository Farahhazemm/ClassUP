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
    // not use lazy loading for repositories.
    // All repositories are added as properties in the Uow 
    // and initial directly in the ctor.

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
       
        public UnitOfWork(  AppDbContext db)
        {
            _db = db;
            Courses = new CourseRepository(_db);
            Lectures = new LectureRepository(_db);
            Categorises = new CategoryRepository(_db);
            Sections =new SectionRepository(_db);
            Reviews = new ReviewRepository(_db);
            Enrollments = new EnrollmentRepository(_db);
            Progresses = new ProgressRepository(_db);
        }
        public ICourseRepository Courses { get; }
        public ILectureRepository Lectures { get; }

        public ICategoryRepository Categorises { get; }

        public ISectionRepository Sections { get; }
        public IReviewRepository Reviews { get; }
        public IEnrollmentRepository Enrollments { get; }

        public IProgressRepository Progresses { get; }

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
