using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;

namespace DemoShop.BLL.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> Register(RegisterRequest request);
    }
}
