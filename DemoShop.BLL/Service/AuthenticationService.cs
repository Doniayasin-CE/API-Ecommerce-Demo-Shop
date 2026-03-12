using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DemoShop.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user,request.Password);
            if(!result.Succeeded)
                return new RegisterResponse() { Success = false, Message = "Error" };
            return new RegisterResponse() { Success = true, Message = "success" };
        }
    }
}
