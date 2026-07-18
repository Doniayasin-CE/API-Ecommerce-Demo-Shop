using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetAllUserOrders(string userId);
        Task<OrderDetailResponse?> GetUserOrderDetails(string userId, int orderId);
        Task<bool> CancelOrder(string userId, int orderId);
        //for Admin
        Task<List<OrderResponse?>> GetOrdersByStatus(OrderStatus status); 
        Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request);
    }
}
