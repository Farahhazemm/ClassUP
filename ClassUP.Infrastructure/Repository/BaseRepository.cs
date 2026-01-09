using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ClassUP.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        private readonly AppDbContext _db;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        #region Read OP
        public async Task<IEnumerable<T>> GetAllAsync(FilterOptions?filter = null)
        {
            filter ??= new FilterOptions();

            IQueryable<T> query = _dbSet.AsNoTracking();

            // Apply filtering
            query = ApplyFilter(query, filter);

            // Apply sorting
            query = ApplySort(query, filter);

            // Apply pagination
            int skip = filter.Skip >= 0 ? filter.Skip : 0;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;
            query = query.Skip(skip).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
        {
           return await _dbSet.AnyAsync(condition); 
        }

        #endregion

        #region Create OP
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            
            return entity;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            
            return entities;

        }
        #endregion

        #region Update OP
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
         
            return entity;
        }

        public async Task<IEnumerable<T>> UpdateManyAsync(IEnumerable<T> entities)
        {
           
            if (!entities.Any())
              return Enumerable.Empty<T>();

            _dbSet.UpdateRange(entities);
        

            return entities;
        }
        #endregion

        #region Delete OP
        public  async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }

        public  async Task<IEnumerable<T>> DeleteManyAsync(IEnumerable<T> entities)
        {
           
            if (!entities.Any())
                return Enumerable.Empty<T>();

            _dbSet.RemoveRange(entities);

            return  entities;
        }

        #endregion

        //  Apply filtering
        private IQueryable<T>ApplyFilter(IQueryable<T> query, FilterOptions filter)
        {
            if (!string.IsNullOrEmpty(filter.FilterBy) && !string.IsNullOrEmpty(filter.FilterValue))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, filter.FilterBy);
                var value = Expression.Constant(filter.FilterValue);
                var condition = Expression.Equal(property, value);
                var predicate = Expression.Lambda<Func<T, bool>>(condition, parameter);

                return query.Where(predicate);
            }
            return query;
        }

        // Helper: Apply sorting
        private IQueryable<T> ApplySort(IQueryable<T> query, FilterOptions filter)
        {
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, filter.SortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = filter.SortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.Type);

                return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;

            }
            return query;

        }




    }
}
