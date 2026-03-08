using DemoShop.DAL.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<T>> GetAllAsync()
        {
            //.Include(c => c.Translations)
            return await _context.Set<T>().ToListAsync();
        }
    }
}
