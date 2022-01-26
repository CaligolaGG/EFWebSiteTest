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
    }
}
