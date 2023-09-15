using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Data.Interfaces;
using TaskManager_Models.Utility;

namespace TaskManager_Data.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private bool disposedValue = false;
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;



        public Repository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(null, nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual T Add(T obj)
        {
            try
            {
                _dbSet.Add(obj);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<T> AddAsync(T obj)
        {
            Add(obj);
            await SaveAsync();
            return obj;
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return _dbSet.Any();
            return _dbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return await _dbSet.AnyAsync();
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual int Save()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Task<int> SaveAsync()
        {
            try
            {
                return _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool Delete(T obj)
        {
            try
            {
                _dbSet.Remove(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginationResult<T>> GetPagedItems(RequestParameters parameters, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool disableTracking = true)
        {
            int MAX_PAGE_SIZE = 50;
            var skip = (parameters.PageNumber - 1) * parameters.PageSize;
            var items = await ConstructQuery(predicate, orderBy, skip, parameters.PageSize, include, disableTracking).ToListAsync();
            return new PaginationResult<T>
            {
                PageSize = items.Count,
                TotalPages = items.Count / MAX_PAGE_SIZE,
                CurrentPage = parameters.PageNumber,
                Records = items,
                TotalRecords = items.Count,
            };
        }

        public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (predicate != null)
                    query = _dbSet.Where(predicate);

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (include != null)
                    query = include(query);

                if (take != null && skip != null)
                    return query.Skip(skip.Value).Take(take.Value);
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IQueryable<T> ConstructQuery(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, int? skip, int? take, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include, bool disableTracking)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking) query = query.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (include != null) query = include(query);

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public virtual bool Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var obj = GetSingleBy(predicate);
                if (obj != null)
                {
                    _dbSet.Remove(obj);
                    return true;
                }
                else
                    throw new Exception($"object does not exist");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task DeleteAsync(T obj)
        {
            Delete(obj);
            await SaveAsync();
        }

        public virtual async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            Delete(predicate);
            await SaveAsync();
        }

        public virtual T GetSingleBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<T> GetSingleByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetQueryableList(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include, bool disableTracking)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query)
                : query;
        }

        public async Task<T?> SingleAsync(Expression<Func<T, bool>> predicate,
      Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
      bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            var result = await query.SingleOrDefaultAsync(predicate);
            return result;
        }

        public virtual T Update(T obj)
        {
            try
            {
                _dbSet.Attach(obj);
                _dbContext.Entry<T>(obj).State = EntityState.Modified;
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<T> UpdateAsync(T obj)
        {
            Update(obj);
            await SaveAsync();
            return obj;
        }


    }
}
