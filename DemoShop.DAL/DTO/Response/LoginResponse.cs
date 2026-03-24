using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Response
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
    }
}
