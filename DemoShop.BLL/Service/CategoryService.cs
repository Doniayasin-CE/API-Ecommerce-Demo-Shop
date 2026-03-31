using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using Mapster;
using System.Linq.Expressions;

namespace DemoShop.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> CreateCategory(CategoryRequest req)
        {
            var category = req.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync(
                new String[] {
                    nameof(Category.Translations),
                    nameof(Category.CreatedBy)
                });
            return categories.Adapt<List<CategoryResponse>>();
        }

        public async Task<CategoryResponse?> GetCategory(Expression<Func<Category,bool>> filter)
        {
            var category = await _categoryRepository.GetOneAsync(filter, new string[] {nameof(Category.Translations)});
            return category.Adapt<CategoryResponse>();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetOneAsync(c => c.Id == id);
            if (category == null) return false;
            return await _categoryRepository.DeleteAsync(category);
        }

    }
}
