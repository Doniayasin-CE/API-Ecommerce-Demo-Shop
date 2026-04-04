using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class ProductTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Language { get; set; } = "en";
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
