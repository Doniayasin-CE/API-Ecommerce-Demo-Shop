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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderResponse>> GetAllUserOrders(string userId)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter: o => o.UserId == userId,
                includes: new string[]
                {
                    nameof(Order.Orderltems), //"Order.Orderltems"
                    $"{nameof(Order.Orderltems)}.{nameof(Orderltem.Product)}", //"Order.Orderltems.Product"
                    $"{nameof(Order.Orderltems)}.{nameof(Orderltem.Product)}.{nameof(Product.Translations)}"
                    //"Order.Orderltems.Product.Translations"
                }
            );

            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<OrderDetailResponse?> GetUserOrderDetails(string userId, int orderId)
        {
            var order = await _orderRepository.GetOneAsync( 
                filter: o => o.UserId == userId && o.Id == orderId,
                includes: new string[]
                {
                    nameof(Order.Orderltems), //"Order.Orderltems"
                    $"{nameof(Order.Orderltems)}.{nameof(Orderltem.Product)}", //"Order.Orderltems.Product"
                    $"{nameof(Order.Orderltems)}.{nameof(Orderltem.Product)}.{nameof(Product.Translations)}"
                    //"Order.Orderltems.Product.Translations"
                }
            );

            if(order == null) return null;

            return order.Adapt<OrderDetailResponse>();
        }

        public async Task<bool> CancelOrder(string userId, int orderId)
        {
            var order = await _orderRepository.GetOneAsync(
                filter: o => o.UserId == userId && o.Id == orderId
            );
            if(order == null) return false;
            if(order.OrderStatus != OrderStatus.Pending) return false;

            order.OrderStatus = OrderStatus.Canceled;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<List<OrderResponse?>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter: o => o.OrderStatus == status
            );
            
            return orders.Adapt<List<OrderResponse?>>();
        }

        public async Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request)
        {
            var order = await _orderRepository.GetOneAsync(o => o.Id == orderId);
            if(order == null) return false;
            
            if (order.OrderStatus == OrderStatus.Canceled
                || order.OrderStatus == OrderStatus.Delivered)
                return false;

            // State Machine Validation: Enforce strict +1 progression
            if((int)request.Status != (int)order.OrderStatus +1)
                return false; //Invalid sequence
            order.OrderStatus= request.Status;
            return await _orderRepository.UpdateAsync(order);
        }
    }
}
