using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }

        public async Task<bool> AddReview(string userId, AddReviewRequest request)
        {
            //Check if the user purchased the item and it was delivered
            var purchasedOrder = await _orderRepository.GetOneAsync(
                filter: o => o.UserId == userId &&
                o.OrderStatus == OrderStatus.Delivered &&
                o.Orderltems.Any(i => i.ProductId == request.ProductId),
                includes: new string[]
                {
                    nameof(Order.Orderltems)
                }
            );
            if (purchasedOrder == null) return false;

            //Check if user already reviewed this product
            var existingReview = await _reviewRepository.GetOneAsync(
                filter: r => r.UserId == userId &&
                r.ProductId == request.ProductId
            );
            if( existingReview != null ) return false;

            //Map request to entity and set secure properties
            var newReview = request.Adapt<Review>();
            newReview.UserId = userId;

            await _reviewRepository.CreateAsync(newReview);
            return true;
        }
    }
}
