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
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.GetCart(userId!);
            if(result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Upsert(AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCart(request, userId!);
            if(!result) return BadRequest();
            return Ok();
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(
            [FromRoute] int productId,
            [FromBody] UpdateCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isUpdated = await _cartService.UpdateCartItemQuantity(productId,request.Count,userId!);
            return isUpdated ? Ok() : BadRequest();
        }

        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isRemoved = await _cartService.ClearCart(userId!);
            if (!isRemoved) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem([FromRoute] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isRemoved = await _cartService.RemoveItem(productId,userId!);
            if(!isRemoved) return BadRequest();
            return Ok();
        }
    }
}
