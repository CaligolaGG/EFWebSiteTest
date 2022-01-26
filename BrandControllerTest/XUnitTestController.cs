using System;
using Xunit;
using EFWebSiteTest.Controllers;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServiceLayer;
using RepoLayer;

namespace BrandControllerTest
{
    public class XUnitTestController
    {
        [Fact]
        public void Test1()
        {

            //var mockService = new Mock<BrandService>();
            //mockService.Setup(x => x.GetPageAsync(1, 5));
            //var controller = new BrandController(mockService.Object);
            
            //// Act
            //IHttpActionResult actionResult = controller.(42);
            //var contentResult = actionResult as OkNegotiatedContentResult<Product>;
        }
    }
}
