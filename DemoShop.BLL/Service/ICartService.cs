using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface ICartService
    {
        Task<bool> AddToCart(AddToCartRequest request, string userId);
        Task<List<CartResponse>> GetCart(string userId);
        Task<bool> UpdateCartItemQuantity(int productId, int targetCount, string userId);
        Task<bool> RemoveItem(int productId, string userId);
        Task<bool> ClearCart(string userId);
    }
}
