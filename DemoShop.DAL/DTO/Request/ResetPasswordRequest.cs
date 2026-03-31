using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.DTO.Request
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
