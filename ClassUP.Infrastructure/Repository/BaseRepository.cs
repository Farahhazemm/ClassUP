using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        #region Read Operations

        public async Task<PaginatedList<T>> GetAllAsync(FilterOptions filter)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            // Apply filtering
            query = ApplyFiltering(query, filter);

            // Apply sorting
            query = ApplySorting(query, filter);

            // Apply pagination 
            return await PaginatedList<T>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        #endregion

        #region Create Operations

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

        #region Update Operations

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

        #region Delete Operations

        public async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> DeleteManyAsync(IEnumerable<T> entities)
        {
            if (!entities.Any())
                return Enumerable.Empty<T>();

            _dbSet.RemoveRange(entities);
            return entities;
        }

        #endregion

        #region Private Helpers

       
        private IQueryable<T> ApplyFiltering(IQueryable<T> query, FilterOptions filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.FilterBy) && !string.IsNullOrWhiteSpace(filter.FilterValue))
            {
            
                var filterBy = filter.FilterBy.Trim();
                var filterValue = $"%{filter.FilterValue.Trim()}%";

                var entityType = typeof(T);
                var property = entityType.GetProperty(filterBy);
                if (property == null) return query;

                // string prope supported for now
                if (property.PropertyType != typeof(string)) return query;

                return query.Where(e => EF.Functions.Like(EF.Property<string>(e, filterBy), filterValue));
            }

            return query;
        }

     
        private IQueryable<T> ApplySorting(IQueryable<T> query, FilterOptions filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sortBy = filter.SortBy.Trim();
                var sortOrder = filter.SortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

                var entityType = typeof(T);
                var property = entityType.GetProperty(sortBy);
                if (property == null) return query.OrderBy(e => EF.Property<object>(e, "Id"));

                if (sortOrder == "OrderBy")
                    return query.OrderBy(e => EF.Property<object>(e, sortBy));
                else
                    return query.OrderByDescending(e => EF.Property<object>(e, sortBy));
            }

            // Default sort id
            var idProp = typeof(T).GetProperty("Id");
            return idProp != null ? query.OrderBy(e => EF.Property<object>(e, "Id")) : query;
        }

       
        #endregion
    }
}