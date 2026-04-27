using DemoShop.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoShop.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,String[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>(); //select * from entity
            
            if(filter != null)  query = query.Where(filter);

            if(includes != null && includes.Length > 0)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include); //.Include(c => c.Translations).Include(p => p.Products)...
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetOneAsync(Expression<Func<T,bool>> filter, String[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include); 
                }
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Update(entity);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> DeleteRangeAsync(List<T> entities)
        {
           _context.RemoveRange(entities);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
