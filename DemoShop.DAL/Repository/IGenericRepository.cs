

using System.Linq.Expressions;

namespace DemoShop.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<List<T>> GetAllAsync(String[]? includes = null);
        Task<T> GetOneAsync(Expression<Func<T, bool>> filter, String[]? includes = null);
    }
}
