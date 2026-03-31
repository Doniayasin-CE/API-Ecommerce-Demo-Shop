using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var res = await _authenticationService.Register(request);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var res = await _authenticationService.Login(request);
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var isConfirmed = await _authenticationService.ConfirmEmail(token, userId);
            if (isConfirmed)
                return Ok();

            return BadRequest();
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordReset(request);
            if(!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> PasswordReset(ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPassword(request);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
