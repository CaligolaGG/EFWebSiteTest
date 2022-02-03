using Microsoft.AspNetCore.Mvc; //!= API Web ASP.NET
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;

namespace EFWebSiteTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly BrandService _brandService;

        public BrandController( BrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _brandService.GetAll();
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
        [HttpGet("BrandPage/{pageNum:int=1}/{pagesize:int:max(10)=5}")]
        public async Task<IActionResult> BrandPage(int pageNum, int pagesize)
        {
            if (pageNum < 1 || pagesize < 1)
                return BadRequest("page num and pagesize must be greater than 0");
            EntityPage<BrandSelect> result = await _brandService.GetPageAsync(pageNum, pagesize);
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
    }

}
