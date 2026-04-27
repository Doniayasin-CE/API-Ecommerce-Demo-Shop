using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using System.Linq.Expressions;


namespace DemoShop.BLL.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategories();
        Task<CategoryResponse> CreateCategory(CategoryRequest req);
        Task<CategoryResponse?> GetCategory(Expression<Func<Category, bool>> filter);
        Task<bool> DeleteCategory(int id);
        Task<bool> ToggleStatus(int id);
    }
}
