using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Response
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; set; } 
        public int Page { get; set; } //current page
        public int Limit { get; set; } //number of items per page
        public int TotalCount { get; set; } //total items
        public int TotalPages => (int) Math.Ceiling((double) TotalCount / Limit);
    }
}
