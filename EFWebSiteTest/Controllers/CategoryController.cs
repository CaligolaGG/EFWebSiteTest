using Microsoft.AspNetCore.Mvc;
using System;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;
using Domain;
using System.Collections.Generic;

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

        /// <summary>
        /// Method to get all the categories from the db
        /// </summary>
        /// <returns>Not Found if no categories are found. Ok() with the list of categories otherwise</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            List<Category> result = await _categoryService.GetAllAsync();
            if (result is null)
                return NotFound("not found");
            return Ok(result);
        }
    }
}
