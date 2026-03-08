using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse> CreateAsync(CategoryRequest req);
    }
}
