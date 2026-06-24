using DemoShop.DAL.Validations;
using Microsoft.AspNetCore.Http;

namespace DemoShop.DAL.DTO.Request
{
    public class ProductRequest
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        [AllowedExtensions(["jpg,png"])]
        [MaxFileSize(4)] // 4MB
        public IFormFile MainImage { get; set; }
        public List<IFormFile>? SubImages { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<ProductTranslationRequest> Translations { get; set; }
    }
}
