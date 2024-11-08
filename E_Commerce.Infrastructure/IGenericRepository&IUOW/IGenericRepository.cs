using System.Linq.Expressions;

namespace E_Commerce.Infrastructure.IGenericRepository_IUOW
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(string id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? orderBy = null, string? direction = null);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Remove(T entity);
        Task UpdateAsync(T entity);
        Task RemoveRange(IEnumerable<T> entities);
        Task<int> Count();
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, params string[] includeProperties);
		Task<T> FindFirstAsync(Expression<Func<T, bool>> expression, params string[] includeProperties);
	}
}
