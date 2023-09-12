using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Data.Interfaces;

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
