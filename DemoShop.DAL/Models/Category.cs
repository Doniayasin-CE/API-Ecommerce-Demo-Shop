using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public List<CategoryTranslation> Translations { get; set; } = null!;
        // public string Status { get; set; } = null!;
    }
}
