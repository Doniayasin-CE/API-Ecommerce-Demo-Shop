using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            var result = await _productService.GetAllProducts(request);
            if (result is null) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetProduct(p => p.Id == id);
            if(result != null) return Ok(result);
            return NotFound();
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            await _productService.CreateProduct(request);
            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateRequest request)
        {
            var result = await _productService.UpdateProduct(id, request);
            if(!result) return BadRequest(result);
            return Ok();
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _productService.ToggleStatus(id);
            if(!result) return NotFound(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if(!result)
                return BadRequest();
            return Ok();
        }
    }
}
