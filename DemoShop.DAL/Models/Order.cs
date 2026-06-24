using DemoShop.DAL.DTO.Request;

namespace DemoShop.DAL.Models
{
    public enum OrderStatus
    {
        Pending = 1,
        Approved = 2,
        Shipped = 3,
        Delivered = 4,
        Canceled = 5,
        Paid = 6,
    }
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? ShippedDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? StripeSessionId { get; set; }
        public decimal? TotalAmount { get; set; }
        // Snapshot Addresses
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public List<Orderltem> Orderltems { get; set; } = new List<Orderltem>();

    }
}
