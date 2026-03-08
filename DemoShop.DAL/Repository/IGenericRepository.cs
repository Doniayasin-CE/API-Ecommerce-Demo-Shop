using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
    }
}
