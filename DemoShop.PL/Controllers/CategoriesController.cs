using DemoShop.BLL.Service;
using DemoShop.DAL.DTO.Request;
using DemoShop.PL.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

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
        public IActionResult Create(CategoryRequest req)
        {
            var res = _categoryService.CreateAsync(req);
            return Ok(new 
            {
                Message = _localizer["Success"].Value,
                Response = res
            });
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var categories = _categoryService.GetAllAsync();
            return Ok(new
            {
                Message = _localizer["Success"].Value,
                Response = categories
            });
        }
    }
}
