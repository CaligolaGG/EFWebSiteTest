using Microsoft.AspNetCore.Mvc;
using System;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;

namespace EFWebSiteTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllAsync();
            if (result is null)
                return NotFound("not found");
            return Ok(result);
        }
    }
}
