using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public string MainImage { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public List<ProductTranslation> Translations { get; set; } = null!;
    }
}
