using Microsoft.AspNetCore.Mvc;
using System.Linq;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;

namespace EFWebSiteTest.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;


        public ProductController(ProductService productService) 
        {
            _productService = productService;
        }

        /// <summary>
        /// api get method for the paging of product
        /// </summary>
        /// <param name="pageNum"> number of the page, starts from 1, should always be positive </param>
        /// <param name="pagesize">size of the page. should always be > 0</param>
        /// <returns>BadRequest when pagenum and pagesize are less than 1.
        /// Not found when the List of product is null or empty.
        /// Ok result with a page of products in any other case.</returns>
        [HttpGet("ProductPage/{pageNum:int=1}/{pagesize:int:max(10)=5}")]
        public async Task<IActionResult> ProductPage(int pageNum, int pagesize)
        {
            if (pageNum < 1 || pagesize < 1)
                return BadRequest("page num and pagesize must be greater than 0");
            EntityPage<ProductSelect> result = await _productService.GetPageAsync(pageNum, pagesize);
            if (result.ListEntities is null || !result.ListEntities.Any())
                return NotFound("page not found");
            return Ok(result);
        }

        /// <summary>
        /// api get method to get a product with some details
        /// </summary>
        /// <param name="productId">id of the product. Should always be > 0</param>
        /// <returns> BadRequest when id is less than 1.
        /// Not found when the object with the specific id has not been found
        /// Ok result with a ProductDetail model in any other case</returns>
        [HttpGet("ProductDetail/{productId:int}")]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            if (productId < 1)
                return BadRequest("id must be greater than 0");
            ProductDetail result = await _productService.GetDetailAsync(productId);
            if (result is null)
                return NotFound("product not found");
            return Ok(result);
        }

    }
}
