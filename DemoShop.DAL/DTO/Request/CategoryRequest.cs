using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Request
{
    public class CategoryRequest
    {
        public List<CategoryTranslationRequest> Translations { get; set; }
    }
}
