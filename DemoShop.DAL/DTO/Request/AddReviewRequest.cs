using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoShop.DAL.DTO.Request
{
    public class AddReviewRequest
    {
        public int ProductId { get; set; }
        public string Comment { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
    }
}
