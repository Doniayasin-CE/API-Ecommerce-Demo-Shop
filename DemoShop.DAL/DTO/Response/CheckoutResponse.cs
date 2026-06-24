using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Response
{
    public class CheckoutResponse
    {
        public int OrderId { get; set; }
        public bool Success { get; set; }
        public string? StripeUrl { get; set; }
        public string? Error { get; set; }
    }
}
