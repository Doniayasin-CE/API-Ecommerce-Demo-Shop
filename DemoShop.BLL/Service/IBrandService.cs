using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface IBrandService
    {
        Task CreateBrand(BrandRequest request);
        Task<List<BrandResponse>> GetAllBrands();
        Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter);
        Task<bool> DeleteBrand(int id);
        Task<bool> ToggleStatus(int id);
    }
}
