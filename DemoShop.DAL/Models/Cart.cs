using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    [PrimaryKey(nameof(ProductId),nameof(UserId))]
    public class Cart
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int Count { get; set; }
    }
}
