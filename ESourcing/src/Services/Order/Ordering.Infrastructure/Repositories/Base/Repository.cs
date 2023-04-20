using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities.Base;
using Ordering.Domain.Repositories.Base;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : IEntityBase
    {
        protected readonly OrderContext _orderContext;
        public Repository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _orderContext.Set<T>().Add(entity);
            await _orderContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _orderContext.Set<T>().Remove(entity);
            await _orderContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _orderContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _orderContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderedBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);
            if (orderedBy != null) return await orderedBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _orderContext.Set<T>().FindAsync(id);
        }

        public async Task Updatesync(T entity)
        {
            _orderContext.Entry<T>(entity).State = EntityState.Modified;
            await _orderContext.SaveChangesAsync();
        }
    }
}
