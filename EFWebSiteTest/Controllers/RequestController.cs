using Microsoft.AspNetCore.Mvc;
using System;
using RepoLayer;
using ServiceLayer;
using System.Threading.Tasks;

namespace EFWebSiteTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestController : Controller
    {
        private readonly InfoRequestService _requestService;

        public RequestController(InfoRequestService requestService) 
        {
            _requestService = requestService;
        }

        /// <summary>
        /// api get method to get a request with some details
        /// </summary>
        /// <param name="requestId">id of the request. Should always be > 0</param>
        /// <returns> BadRequest when id is less than 1.
        /// Not found when the object with the specific id has not been found.
        /// Ok result with a RequestDetail model in any other case</returns>
        [HttpGet("RequestDetail/{requestId:int}")]
        public async Task<IActionResult> RequestDetail(int requestId)
        {
            if (requestId < 1)
                return BadRequest("id must be greater than 0");
            RequestDetail result = await _requestService.GetDetailAsync(requestId);
            if (result is null)
                return NotFound(String.Format("request {0} not found", requestId));
            return Ok(result);
        }

        [HttpGet()]
        /// <summary>
        /// api get method for the paging of product
        /// </summary>
        /// <param name="pageNum"> number of the page, starts from 1, should always be positive </param>
        /// <param name="pagesize">size of the page. should always be > 0</param>
        /// <returns>BadRequest when pagenum and pagesize are less than 1.
        /// Not found when the List of product is null or empty.
        /// Ok result with a page of products in any other case.</returns>
        [HttpGet("LeadsPage/{pageNum:int=1}/{pagesize:int:max(10)=5}/{productName=}/{brandName=}")]
        public async Task<IActionResult> ProductPage(int pageNum, int pagesize, string productName, string brandName)
        {
            if (pageNum < 1 || pagesize < 1)
                return BadRequest("page num and pagesize must be greater than 0");
            EntityPage<RequestSelect> result = await _requestService.GetPageAsync(pageNum, pagesize, productName, brandName);
            if (result.ListEntities is null) // || !result.ListEntities.Any())
                return NotFound("page not found");
            return Ok(result);
        }
    }
}
