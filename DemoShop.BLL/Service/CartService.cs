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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddToCart(AddToCartRequest request, string userId)
        {
            var product = await _productRepository.GetOneAsync(p=>p.Id == request.ProductId);
            if(product == null) return false;

            var existingItem = await _cartRepository.GetOneAsync(
                c => c.ProductId == request.ProductId 
                && c.UserId == userId
            );
            var currentCount = existingItem?.Count ?? 0;
            var newCount = currentCount + request.Count;
            if (newCount > product.Quantity) return false;

            if(existingItem != null)
            {
                existingItem.Count = newCount;
                await _cartRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newCartItem = request.Adapt<Cart>();
                newCartItem.UserId = userId;
                await _cartRepository.CreateAsync(newCartItem);
            }
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var items = await _cartRepository.GetAllAsync(c => c.UserId == userId);
            if(!items.Any()) return false;

            await _cartRepository.DeleteRangeAsync(items);
            return true;
        }

        public async Task<List<CartResponse>> GetCart(string userId)
        {
            var items = await _cartRepository.GetAllAsync(
                filter: c => c.UserId == userId,
                includes: new string[]
                {
                    nameof(Cart.Product),
                    $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                }
            );

            return items.Adapt<List<CartResponse>>();
        }

        public async Task<bool> RemoveItem(int productId, string userId)
        {
            var item = await _cartRepository.GetOneAsync(
                c => c.ProductId == productId && c.UserId == userId
            );
            if(item == null) return false;
            
            return await _cartRepository.DeleteAsync(item);
        }

        public async Task<bool> UpdateCartItemQuantity(int productId, int targetCount, string userId)
        {
            var item = await _cartRepository.GetOneAsync(
                c => c.ProductId == productId && c.UserId == userId
            );
            if(item == null) return false;

            var product = await _productRepository.GetOneAsync(p => p.Id == productId);
            if(product == null || targetCount > product.Quantity) return false;

            item.Count = targetCount;
            return await _cartRepository.UpdateAsync(item);
        }
    }
}
