using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ServiceLayer;
using System.Threading.Tasks;
using Domain;
using System;
using System.Collections.Generic;

namespace EFWebSiteTest.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ProductCategoryService _productcategoryService;


        public ProductController(ProductService productService, ProductCategoryService productcategoryService) 
        {
            _productService = productService;
            _productcategoryService = productcategoryService;
        }

        /// <summary>
        /// api get method for the paging of product
        /// </summary>
        /// <param name="pageNum"> number of the page, starts from 1, should always be positive </param>
        /// <param name="pagesize">size of the page. should always be > 0</param>
        /// <returns>BadRequest when pagenum and pagesize are less than 1.
        /// Not found when the List of product is null or empty.
        /// Ok result with a page of products in any other case.</returns>
        [HttpGet("ProductPage/{pageNum:int=1}/{pagesize:int:max(10)=5}/{orderBy=0}/{isAsc=true}/{brandName=}")]
        public async Task<IActionResult> ProductPage(int pageNum, int pagesize, int orderBy, bool isAsc, string brandName)
        {
            if (pageNum < 1 || pagesize < 1)
                return BadRequest("page num and pagesize must be greater than 0");
            EntityPage<ProductSelect> result = await _productService.GetProductPageAsync(pageNum, pagesize,orderBy,isAsc,brandName);
            if (result.ListEntities is null || !result.ListEntities.Any())
                return NotFound("page not found");
            return Ok(result);
        }

        /// <summary>
        /// api get method to get a product with its categories
        /// </summary>
        /// <param name="productId">id of the product. Should always be > 0</param>
        /// <returns> BadRequest when id is less than 1.
        /// Not found when the object with the specific id has not been found
        /// Ok result with a ProductDetail model in any other case</returns>
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProductAndCategories(int productId)
        {
            if (productId < 1)
                return BadRequest("invalid id");
            var result = await _productService.GetProductAsync(productId);
            if (result is null)
                return NotFound();
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
            ProductDetail result = await _productService.GetProductDetailAsync(productId);
            if (result is null)
                return NotFound("product not found");
            return Ok(result);
        }

        /// <summary>
        /// Api post method to create a Product with the categories.
        /// </summary>
        /// <param name="model">List of the of ProductAndCategoryModel which holds both 
        /// the product to create and the list of categories associated </param>
        [HttpPost("InsertProductCat")]
        public async Task<IActionResult> InsertProductWithCategories (ProductAndCategoryModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await InsertOrUpdateProduct(model.Product);
            List<ProductCategory> productCategories = new List<ProductCategory>();
            foreach (int category in model.Categories)
                productCategories.Add(new ProductCategory { IdCategory = category, IdProduct = model.Product.Id });
            await _productcategoryService.InsertMultiple(productCategories);
            return result;
        }


        /// <summary>
        /// Api put method to create a Product with the categories.
        /// </summary>
        /// <param name="model">List of the of ProductAndCategoryModel which holds both 
        /// the product to create and the list of categories associated </param>
        [HttpPut("UpdateProductCat")]
        public async Task<IActionResult> UpdateProductWithCategories(ProductAndCategoryModel model)
        {
            if(model.Product.Name.Length > 50 || model.Product.Description.Length > 50 || model.Product.ShortDescription.Length > 20)
                return BadRequest("inputs too longs");
            if (!ModelState.IsValid || model.Product.Id < 1)
                return BadRequest(ModelState);

            var result = await InsertOrUpdateProduct(model.Product);
            List<ProductCategory> productCategories = new List<ProductCategory>();
            foreach (int category in model.Categories)
                productCategories.Add(new ProductCategory { IdCategory = category, IdProduct = model.Product.Id });
            
            await _productcategoryService.UpdateMultiple(productCategories);
            return result;
        }


        /// <summary>
        /// Api post method update a  Product. 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Bad request if the product inserted is null, doesnt have a brand or a valid name, or the price is negative.
        /// Forbid if the product has not been inserted by the db for any reason
        /// Created with the product created, if the product got created successfully
        /// </returns>
        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct(Product product) => await InsertOrUpdateProduct(product);


        /// <summary>
        /// Api post method update a  Product. 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Bad request if the product inserted is null, doesnt have a brand or a valid name, or the price is negative.
        /// Forbid if the product has not been inserted by the db for any reason
        /// Created with the product created, if the product got created successfully
        /// </returns>
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Product product) => await InsertOrUpdateProduct(product);


        /// <summary>
        /// Method to insert or update a new Product. 
        /// Insert if the product id is 0 (not set). Update if the id is != 0
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Bad request if the product inserted is null, doesnt have a brand or a valid name, or the price is negative.
        /// Forbid if the product has not been inserted by the db for any reason
        /// Created with the product created, if the product got created successfully
        /// </returns>
        public async Task<IActionResult> InsertOrUpdateProduct(Product product) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (product is null || product.BrandId < 1 || String.IsNullOrEmpty(product.Name) || product.Price < 0)
                return BadRequest("product not valid");

            if (await _productService.InsertOrUpdateAsync(product) < 1)
                return Forbid("product has not been inserted");
            return Ok(product);
        }




        /// <summary>
        /// Api post method to Remove a  Product given its id.
        /// </summary>
        /// <param name="productId">the product id</param>
        /// <returns>
        /// BadRequest if the id provided is less then 1
        /// NotFound if the call to the repo returns 0, therefore the product was either not found or not deleted
        /// Ok otherwise
        /// </returns>
        [HttpDelete("DeleteProduct/{productId:min(1)}")]
        public async Task<IActionResult> DeleteProduct(int productId) 
        {
            if (productId < 1)
                return BadRequest("product must be > 0");
            
            var result = await _productService.DeleteAsync(productId);
            if (result < 1)
                return NotFound("product has not been found or not been deleted");

            return Ok();
        }

        [HttpDelete("DeleteProductL/{productId:min(1)}")]
        public async Task<IActionResult> DeleteProductLogical(int productId)
        {
            if (productId < 1)
                return BadRequest("product must be > 0");

            var result = await _productService.DeleteLogicalAsync(productId);
            if (result < 1)
                return NotFound("product has not been found or not been deleted");

            return Ok(result);
        }

    }
}
