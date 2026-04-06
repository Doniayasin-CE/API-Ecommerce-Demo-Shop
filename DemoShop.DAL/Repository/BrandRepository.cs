using DemoShop.DAL.Data;
using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Repository
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) 
        : base(context)
        {
        }
    }
}
