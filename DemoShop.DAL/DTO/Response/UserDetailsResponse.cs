using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Response
{
    public class UserDetailsResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public bool IsBlocked { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
