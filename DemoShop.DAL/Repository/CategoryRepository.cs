using DemoShop.DAL.Data;
using DemoShop.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoShop.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        { 
            _context = context;
        }

        Category ICategoryRepository.Create(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
            return category;
        }

        List<Category> ICategoryRepository.GetAll()
        {
            var categories = _context.Categories.Include(c => c.Translations).ToList();
            return categories;
        }
    }
}
