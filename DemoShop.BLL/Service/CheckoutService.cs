using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;

        public CheckoutService(ICartRepository cartRepository, 
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            ICartService cartService,
            IProductRepository productRepository,
            IEmailSender emailSender)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _cartService = cartService;
            _productRepository = productRepository;
            _emailSender = emailSender;
        }
        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetAllAsync(
                filter: c => c.UserId == userId,
                includes: new string[]
                {
                    nameof(Cart.Product),
                    $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                }
            );
            if (!cartItems.Any())
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Cart is empty"
                };
            var user = await _userManager.FindByIdAsync(userId);
            var city = request.City ?? user!.City;
            if (city == null)
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "City is required"
                };
            var street = request.Street ?? user!.Street;
            if (street == null)
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Street is required"
                };
            var phoneNum = request.PhoneNumber ?? user!.PhoneNumber;
            if (phoneNum == null)
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "PhoneNumber is required"
                };
            foreach(var item in cartItems)
            {
                if (item.Count > item.Product.Quantity)
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "Not enough stock"
                    };
            }

            // Create Order with Order items
            var order = new Order()
            {
                UserId = userId,
                City = city,
                Street = street,
                PhoneNumber = phoneNum,
                PaymentMethod = request.PaymentMethod,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Count),
                Orderltems = cartItems.Select(c => new Orderltem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Count,
                    UnitPrice = c.Product.Price,
                    TotalPrice = c.Product.Price * c.Count
                }).ToList()
            };
            await _orderRepository.CreateAsync(order);

            if(request.PaymentMethod == PaymentMethod.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true
                };
            }

            if(request.PaymentMethod == PaymentMethod.Visa)
            {
                var scheme = _httpContextAccessor.HttpContext!.Request.Scheme;
                var host = _httpContextAccessor.HttpContext!.Request.Host;
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    SuccessUrl = $"{scheme}://{host}/api/Checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{scheme}://{host}/api/Checkouts/cancel",
                    LineItems = new List<SessionLineItemOptions>()
                };

                foreach(var item in cartItems)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en")!.Name,
                            },
                            UnitAmount = (long) (item.Product.Price * 100),
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionListItem);
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.StripeSessionId = session.Id;
                await _orderRepository.UpdateAsync(order);
                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url
                };
            }

            return new CheckoutResponse
            {
                Success = false,
                Error = "faild checkout process!!"
            };
        }

        public async Task<CheckoutResponse> HandleSuccessPayment(string sessionId)
        {
            var order = await _orderRepository.GetOneAsync(
                filter: o => o.StripeSessionId == sessionId,
                includes: new[]
                {
                    nameof(Order.Orderltems),
                    $"{nameof(Order.Orderltems)}.{nameof(Orderltem.Product)}",
                    $"{nameof(Order.Orderltems)}.{nameof(Orderltem.Product)}.{nameof(Product.Translations)}"
                } 
            );
            if (order is null)
                return new CheckoutResponse { Success = false, Error = "Order not found!" };
            order.OrderStatus = OrderStatus.Paid;
            await _orderRepository.UpdateAsync(order);

            //clean cart
            await _cartService.ClearCart(order.UserId);

            //User Notifications 
            var user = await _userManager.FindByIdAsync(order.UserId);
            await _emailSender.SendEmailAsync(
                email: user!.Email!,
                subject: "Order Confirmation",
                message: "<h2> Your order has been confirmed successfully </h2>"
            );
            //inventory managment
            //Decrease stock Quantity of the product and update the products list
            var LowStockProducts = await _productRepository.DecreaseQuantityAsync(order.Orderltems);
            if (LowStockProducts != null)
            {
                foreach (var product in LowStockProducts)
                {
                    //Dispatch administrative notification
                    await _emailSender.SendEmailAsync(
                        email: "donia.yasin21@gmail.com",
                        subject: "Low Stock Alert",
                        message: $"<h2> Warning: The Product {product.Translations.FirstOrDefault(t => t.Language == "en")!.Name} " +
                        $"with ID:{product.Id} " +
                        $"has {product.Quantity} units left </h2>"
                    );
                }
            }
            return new CheckoutResponse { Success = true, OrderId = order.Id };
        }
    }
}
