using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.Helpers.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        #region Read Methods
        Task<PaginatedList<T>> GetAllAsync(FilterOptions filter);
        Task<T?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        #endregion

        #region Create Methods
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities);
        #endregion

        #region Update Methods
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> UpdateManyAsync(IEnumerable<T> entities);
        #endregion

        #region Delete Methods
        Task<T> DeleteAsync(T entity);
        Task<IEnumerable<T>> DeleteManyAsync(IEnumerable<T> entities);
        #endregion
    }
}