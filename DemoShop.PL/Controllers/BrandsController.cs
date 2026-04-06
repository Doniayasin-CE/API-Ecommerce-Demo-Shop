using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var result = await _brandService.GetAllBrands();
            if (result.Count < 0)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _brandService.GetBrand(b => b.Id == id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] BrandRequest request)
        {
            await _brandService.CreateBrand(request);
            return Ok(request);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandService.DeleteBrand(id);
            if(!result)
                return NotFound();
            return Ok();
        }
    }
}
