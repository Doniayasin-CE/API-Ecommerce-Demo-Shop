using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoShop.PL.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Admin/Users")]
    [ApiController]
    [Authorize]
    //[Authorize(Roles = "SuperAdmin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("")]
        public async Task<IActionResult> ListUsers()
        {
            var result = await _userManagementService.GetAllUsers();
            if(result == null) return BadRequest(result);
            return Ok(new {Users = result});
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ListUserDetails([FromRoute] string userId)
        {
            var result = await _userManagementService.GetUserDetails(userId);
            if(result == null) return NotFound(result);
            return Ok(new {User =  result});
        }

        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> ChangeUsreRole([FromRoute] string userId,
            [FromBody] ChangeRoleRequest request)
        {
            var result = await _userManagementService.ChangeUsreRole(userId, request.NewRole);
            if(!result) return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{userId}/Toggle-Block")]
        public async Task<IActionResult> BlockUser([FromRoute] string userId)
        {
            var result = await _userManagementService.ToggleBlockUser(userId);
            if (!result) return BadRequest(result);
            return Ok(result);
        }
    }
}
