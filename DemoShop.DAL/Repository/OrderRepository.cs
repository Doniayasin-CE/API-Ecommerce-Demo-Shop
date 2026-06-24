using DemoShop.DAL.Data;
using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
