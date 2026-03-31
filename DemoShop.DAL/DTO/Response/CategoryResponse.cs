using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string UserCreated { get; set; }
        public string CategoryName { get; set; }
        //public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
