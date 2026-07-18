using DemoShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface IUserManagementService
    {
        Task<List<UserListResponse>> GetAllUsers();
        Task<UserDetailsResponse> GetUserDetails(string userId);
        Task<bool> ChangeUsreRole(string userId, string role);
        Task<bool> ToggleBlockUser(string userId);
        Task<bool> DeleteUser(string userId);
    }
}
