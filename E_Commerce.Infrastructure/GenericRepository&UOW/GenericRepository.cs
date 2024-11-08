using E_Commerce.Data.Consts;
using E_Commerce.Data.Data;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Infrastructure.GenericRepository_UOW
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        protected readonly E_CommerceContext _context;
        public GenericRepository(E_CommerceContext context)  
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, params string[] includeProperties)
		{
			IQueryable<T> query = _context.Set<T>().Where(expression);
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty);
				}
			}
			return await query.ToListAsync();
		}


		public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expression, params string[] includeProperties)
		{
			IQueryable<T> query = _context.Set<T>().Where(expression);
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty);
				}
			}
			return await query.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? orderBy = null, string? direction = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (orderBy != null)
            {
                if (direction == OrderDirection.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return query.ToList();
        }
        public T GetById(string id)
        {

            return _context.Set<T>().Find(id);
        }
		public async Task UpdateAsync(T entity)
		{
			_context.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}
		public async Task Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }
    }
}
