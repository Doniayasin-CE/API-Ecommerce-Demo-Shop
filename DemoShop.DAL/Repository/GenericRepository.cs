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

        public async Task<List<T>> GetAllAsync(String[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>(); //select * from entity
            if(includes != null && includes.Length > 0)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include); //.Include(c => c.Translations).Include(p => p.Products)...
                }
            }
            
            return await query.ToListAsync();
        }

        public async Task<T> GetOneAsync(Expression<Func<T,bool>> filter, String[]? includes = null)
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
    }
}
