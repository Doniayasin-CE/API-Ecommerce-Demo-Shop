using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> UserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _orderService.GetAllUserOrders(userId!);
            if(result == null) return NotFound(result);
            return Ok(new { Orders = result });
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> UserOrderDetails([FromRoute] int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _orderService.GetUserOrderDetails(userId!,orderId);
            if( result == null) return NotFound(result);
            return Ok(new {Order =  result});
        }

        [HttpPatch("{orderId}")]
        [Authorize]
        public async Task<IActionResult> OrderCancellation([FromRoute] int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _orderService.CancelOrder(userId!,orderId);
            if(!result) return NotFound(result);
            return Ok();
        }

        [HttpGet("admin")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatus status = OrderStatus.Pending)
        {
            var result = await _orderService.GetOrdersByStatus(status);
            if(result == null) return NotFound(result);
            return Ok(new{Orders = result});
        }

        [HttpPatch("admin/{orderId}/status")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeStatus(int orderId, [FromBody] ChangeOrderStatusRequest request)
        {
            var result = await _orderService.ChangeOrderStatus(orderId, request);
            if(!result) return BadRequest(result);
            return Ok(result);
        }

    }
}
