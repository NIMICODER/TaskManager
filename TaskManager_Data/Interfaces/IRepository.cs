using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Data.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T obj);

        Task<T> AddAsync(T obj);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);

        bool Any(Expression<Func<T, bool>> predicate = null);

        int Save();

        Task<int> SaveAsync();

        T GetSingleBy(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleByAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetQueryableList(
       Expression<Func<T, bool>>? predicate = null,
       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
       Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
       bool disableTracking = true);

        Task<T?> SingleAsync(Expression<Func<T, bool>> predicate,
       Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
       bool disableTracking = true);

        T Update(T obj);

        Task<T> UpdateAsync(T obj);

    }
}
