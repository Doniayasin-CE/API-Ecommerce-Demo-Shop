using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Language { get; set; } = "en";

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
