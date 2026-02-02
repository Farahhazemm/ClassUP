using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ClassUP.Infrastructure.Repository
{
    internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext db) : base(db)
        {
        }

      
    }
}
