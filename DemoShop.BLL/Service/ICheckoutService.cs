using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request);
        Task<CheckoutResponse> HandleSuccessPayment(string sessionId);
    }
}
