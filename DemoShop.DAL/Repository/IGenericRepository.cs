

using System.Linq.Expressions;

namespace DemoShop.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, String[]? includes = null);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, String[]? includes = null);
        Task<bool> DeleteAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteRangeAsync(List<T> entities);
    }
}
