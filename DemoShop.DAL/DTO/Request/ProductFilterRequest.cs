using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Request
{
    public class ProductFilterRequest : PaginationRequest
    {
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public int? MinimumRate { get; set; }
        public int? MaximumRate { get; set; }
    }
}
