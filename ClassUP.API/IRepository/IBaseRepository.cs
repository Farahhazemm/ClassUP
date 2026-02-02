using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        #region Read Methods
        Task<IEnumerable<T>> GetAllAsync(FilterOptions? op);
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

        #region Dleate Methods
        Task<T> DeleteAsync(T entity);
        Task<IEnumerable<T>> DeleteManyAsync(IEnumerable<T> entities);


        #endregion
    }
}
