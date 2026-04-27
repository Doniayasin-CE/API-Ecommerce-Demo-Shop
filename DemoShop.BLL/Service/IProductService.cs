using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface IProductService
    {
        Task CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProducts();
        Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter);
        Task<bool> DeleteProduct(int id);
        Task<bool> UpdateProduct(int id, ProductUpdateRequest request);
        Task<bool> ToggleStatus(int id);
    }
}
