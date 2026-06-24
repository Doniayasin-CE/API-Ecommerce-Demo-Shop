using DemoShop.DAL.Data;
using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<List<Product>?> DecreaseQuantityAsync(List<Orderltem> orderltems)
        {
            var porductIds = orderltems.Select(i => i.ProductId).ToList();
            var porducts = await GetAllAsync(p => porductIds.Contains(p.Id));
            
            foreach(var product in porducts)
            {
                var item = orderltems.FirstOrDefault(p => p.ProductId == product.Id);
                product.Quantity -= item!.Quantity;
            }
            await UpdateRangeAsync(porducts);
            return porducts.Where(p => p.Quantity < 5).ToList();
        }

        //public async Task<bool> DecreaseQuantityAsync(int productId, int amount)
        //{
        //    var product = await GetOneAsync(p => p.Id == productId);
        //    product!.Quantity -= amount;
        //    await UpdateAsync(product);
        //    //Low stock detection threshold
        //    return product.Quantity < 5;
        //}
    }
}
