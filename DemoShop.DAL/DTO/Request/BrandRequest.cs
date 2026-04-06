using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Request
{
    public class BrandRequest
    {
        public IFormFile MainLogo { get; set; }
        public List<BrandTranslationRequest> Translations { get; set; }
    }
}
