using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class Brand : AuditableEntity
    {
        public int Id { get; set; }
        public string MainLogo { get; set; } = null!;
        public List<BrandTranslation> Translations { get; set; } = null!;
        public List<Product> Products { get; set; } = null!;
    }
}
