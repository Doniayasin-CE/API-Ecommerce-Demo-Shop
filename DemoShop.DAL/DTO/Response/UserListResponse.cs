using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Response
{
    public class UserListResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
    }
}
