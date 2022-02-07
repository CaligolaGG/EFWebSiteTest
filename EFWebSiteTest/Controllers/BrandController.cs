﻿using Microsoft.AspNetCore.Mvc; //!= API Web ASP.NET
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;
using Domain;

namespace EFWebSiteTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly BrandService _brandService;


        /// <summary>
        /// Method to get all the brands from the db
        /// </summary>
        /// <returns>Not Found if no brands are found. Ok() with the list of brands otherwise</returns>

        public BrandController( BrandService brandService)
        {
            _brandService = brandService;
        }

        
        [HttpGet("GetNames")]
        public async Task<IActionResult> GetAllBrandsNames()
        {
            var result = await _brandService.GetAllNamesAsync();
            if(result is null)
                return NotFound("not found");
            return Ok(result);
        }


        /// <summary>
        /// api get method for the paging of Brand
        /// </summary>
        /// <param name="pageNum"> number of the page, starts from 1, should always be positive </param>
        /// <param name="pagesize">size of the page. should always be > 0</param>
        /// <returns> BadRequest when pagenum and pagesize are less than 1.
        /// Not found when the List of brands is null or empty.
        /// Ok result with a page of brands in any other case </returns>
        [HttpGet("BrandPage/{pageNum:int=1}/{pagesize:int:max(10)=5}/{searchByName=}")]
        public async Task<IActionResult> BrandPage(int pageNum, int pagesize, string searchByName)
        {
            if (pageNum < 1 || pagesize < 1)
                return BadRequest("page num and pagesize must be greater than 0");
            EntityPage<BrandSelect> result = await _brandService.GetPageAsync(pageNum, pagesize, searchByName);
            if (result.ListEntities is null || !result.ListEntities.Any())
                return NotFound("page not found");
            return Ok(result);
        }

        /// <summary>
        /// api get method to get a brand with some details
        /// </summary>
        /// <param name="brandId">id of the request. Should always be > 0</param>
        /// <returns> BadRequest when id is less than 1.
        /// Not found when the object with the specific id has not been found
        /// Ok result with a BrandDetail model in any other case</returns>
        [HttpGet("BrandDetail/{brandId:int}")]
        public async Task<IActionResult> BrandDetail(int brandId)
        {
            if (brandId < 1)
                return BadRequest("id must be greater than 0");
            BrandDetail result = await _brandService.GetDetailAsync(brandId);
            if (result is null)
                return NotFound("brand not found");
            return Ok(result);
        }

        /// <summary>
        /// api method to update a brand
        /// </summary>
        /// <param name="brand">brand to update</param>
        /// <returns>
        /// BadRequest if the brand object or its properties are null
        /// NotFound if the brand doesnt get updated for any reason
        /// Ok() otherwise
        /// </returns>
        [HttpPut("BrandUpdate")]
        public async Task<IActionResult> BrandUpdate (Brand brand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (brand.Id < 0 || brand is null|| String.IsNullOrEmpty(brand.BrandName))
                return BadRequest("invalid brand selected");
            int numChangedRows = await _brandService.BrandUpdateAsync(brand);
            if (numChangedRows < 1)
                return NotFound("brand not updated");
            return Ok(brand);
        }

        /// <summary>
        /// api method to logically delete a brand
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns> 
        /// BadRequest if the brandId is less then 1
        /// NotFound if the brand doesnt get updated for any reason
        /// Ok() otherwise
        /// </returns>
        [HttpDelete("BrandDelete")]
        public async Task<IActionResult> DeleteLogicalAsync(int brandId) 
        {
            if (brandId < 1)
                return BadRequest("invalid brand id. Id must be greater than 0");
            if (await _brandService.BrandDeleteLogicalAsync(brandId) < 1)
                return NotFound("brand not deleted");
            return Ok();
        }


        /// <summary>
        /// Api method to insert a new brand without specifing its products
        /// </summary>
        /// <param name="brand">brand to add</param>
        /// <returns>
        /// Bad request if the brand inserted is not valid
        /// Forbid if the brand has not been inserted for any reason
        /// Ok with the brand inserted otherwise
        /// </returns>
        [HttpPost("BrandCreate")]
        public async Task<IActionResult> CreateBrandAsync(Brand brand)
        {
            if (!ModelState.IsValid || String.IsNullOrWhiteSpace(brand.BrandName))
                return BadRequest(ModelState);
            if(await _brandService.CreateBrandAsync(brand) < 1)
                return Forbid("brand has not been inserted");
            return Ok(brand);
        }


        /// <summary>
        /// Api method to insert a new brand with its products and associated categories
        /// </summary>
        /// <param name="brandWithProducts">model passed to the api</param>
        /// <returns>
        /// Bad request if the brand inserted is not valid
        /// Forbid if the brand has not been inserted for any reason
        /// Ok with the model inserted otherwise
        /// </returns>
        public async Task<IActionResult> CreateBrandWithProductsAsync(BrandWithProducts brandWithProducts)
        {
            if (!ModelState.IsValid || String.IsNullOrWhiteSpace(brandWithProducts.Brand.BrandName))
                return BadRequest(ModelState);
            if (await _brandService.CreateBrandWithProductsAsync(brandWithProducts) < 1)
                return Forbid("brand has not been inserted");
            return Ok(brandWithProducts);
        }

    }



}
