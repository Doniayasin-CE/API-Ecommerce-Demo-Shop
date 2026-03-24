using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using DemoShop.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResources> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create(CategoryRequest req)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _categoryService.CreateCategory(req);
            return Ok(new 
            {
                Message = _localizer["Success"].Value,
                Response = res
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(new
            {
                Message = _localizer["Success"].Value,
                Response = categories
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _categoryService.GetCategory(c => c.Id == id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _categoryService.DeleteCategory(id);
            if (!isDeleted)
            {
                return NotFound(new { Message = _localizer["NotFound"].Value });
            }
            return Ok(new { Message = _localizer["Success"].Value });
        }
    }
}
