using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DemoShop.DAL.DTO.Request
{
    public enum PaymentMethod
    {
        Cash = 1,
        Visa = 2,
    }
    public class CheckoutRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
