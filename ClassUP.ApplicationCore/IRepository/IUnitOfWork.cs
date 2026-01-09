using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IUnitOfWork
    {
        IBaseRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
