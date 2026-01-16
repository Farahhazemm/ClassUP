using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }

        Task<int> SaveChangesAsync();
    }
}
