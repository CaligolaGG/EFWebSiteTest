using Microsoft.AspNetCore.Mvc;
using System.Linq;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;
using Domain;
using System;
using System.Collections.Generic;

namespace EFWebSiteTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCategoryController : Controller
    {

        private readonly ProductCategoryService _productCategoryService;
        public ProductCategoryController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        /// <summary>
        /// Api post method to insert a new ProductCategory
        /// </summary>
        /// <param name="productCategories"></param>
        /// <returns>Bad request if the ProductCategory inserted is null.
        /// Forbid if the ProductCategory has not been inserted by the db for any reason
        /// Created with the ProductCategory created, if the ProductCategory got created successfully
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> InsertNew(List<ProductCategory> productCategories)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (productCategories is null)
                return BadRequest("list is not valid");
            var result = await _productCategoryService.InsertMultiple(productCategories);
            if ( result < 1)
                return Forbid("no productCategory have been inserted");
            if(result < productCategories.Count)
                return Forbid(result+" out of "+ productCategories.Count + " have been inserted");

            return Created("ProductCategory", productCategories);
        }
    }
}
