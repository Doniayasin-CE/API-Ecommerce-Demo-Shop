using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class BrandTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Language { get; set; } = "en";
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
    }
}
