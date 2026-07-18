using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
