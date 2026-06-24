using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>?> DecreaseQuantityAsync(List<Orderltem> orderltems);
        //Task<bool> DecreaseQuantityAsync(int productId, int amount); 
    }
}
