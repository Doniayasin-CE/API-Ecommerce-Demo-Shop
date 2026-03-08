using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> CreateAsync(CategoryRequest req)
        {
            var category = req.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Adapt<List<CategoryResponse>>();
        }
    }
}
