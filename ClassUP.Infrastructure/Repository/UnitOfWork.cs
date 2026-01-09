using ClassUP.ApplicationCore.IRepository;
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
        private readonly Dictionary<Type, object> _repositories = new();
        public UnitOfWork(  AppDbContext db)
        {
            _db = db;
        }
        public IBaseRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out var repo))
            {
                repo = new BaseRepository<T>(_db);
                _repositories[type] = repo;
            }

            return (IBaseRepository<T>)repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
