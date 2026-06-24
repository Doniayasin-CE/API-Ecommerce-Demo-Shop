using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _checkoutService.ProcessCheckout(userId!, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> SuccessPayment([FromQuery] string sessionId)
        {
            if (sessionId == null) return BadRequest("Session ID is required");
            var result = await _checkoutService.HandleSuccessPayment(sessionId);
            if (!result.Success) return BadRequest(result.Error);
            return Ok(new {message = "Payment Successful", result.OrderId});
        }

        //SuccessUrl = $"{scheme}://{host}/checkout/success"
        //CancelUrl = $"{scheme}://{host}/checkout/cancel",
    }
}
