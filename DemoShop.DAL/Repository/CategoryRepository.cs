using DemoShop.DAL.Data;
using DemoShop.DAL.Models;

namespace DemoShop.DAL.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
