using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        ILectureRepository Lectures { get; }
        ICategoryRepository Categorises { get; }

        ISectionRepository Sections { get; }
        IReviewRepository Reviews { get; }
        Task<int> SaveChangesAsync();
    }
}
