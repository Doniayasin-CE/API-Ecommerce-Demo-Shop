using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using DemoShop.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DemoShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ICategoryRepository categoryRepository, IStringLocalizer<SharedResources> localizer)
        {
            _categoryRepository = categoryRepository;
            _localizer = localizer;
        }

        [HttpPost]
        public IActionResult Create(CategoryRequest req)
        {
            var category = req.Adapt<Category>();
            _categoryRepository.Create(category);
            return Ok(new {Message = _localizer["Success"].Value});
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _categoryRepository.GetAll();
            var res = categories.Adapt<List<CategoryResponse>>();
            return Ok(new {
                Data = res,
                Message = _localizer["Success"].Value});
        }
    }
}
