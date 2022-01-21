using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;


namespace EFWebSiteTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private MyDbContext _ctx;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [HttpGet]
        public List<Account> GetAll() => _ctx.Accounts.ToList();
        #region getalls
        //public List<Product> Get() => _ctx.Products.ToList();
        //public List<Nation> Get() => _ctx.Nations.ToList();
        //public List<Category> Get() => _ctx.Categories.ToList();
        //public List<User> Get() => _ctx.Users.ToList();
        //public List<Brand> Get() => _ctx.Brands.ToList();
        //public List<ProductCategory> GetAll() => _ctx.ProductCategories.ToList(); 
        //public List<InfoRequestReply> Get() => _ctx.InfoRequestReplies.ToList(); 
        //public List<InfoRequest> Get() => _ctx.InfoRequests.ToList();
        #endregion

        [HttpGet("Product/{pageNum}")]
        public IActionResult ProductPage(int pageNum, int pagesize)
        {
            ProductService productService = new ProductService(_ctx);
            EntityPage<ProductSelect> productPageTemp = productService.GetProductPage(pageNum, pagesize);

            return Ok(productPageTemp);
        }

        [HttpGet("Brand/{pageNum}")]
        public IActionResult BrandPage(int pageNum, int pagesize)
        {
            BrandService Service = new BrandService(_ctx);
            EntityPage<BrandSelect> productPageTemp = Service.GetBrandPage(pageNum, pagesize);

            return Ok(productPageTemp);
        }

        [HttpGet("ProductDetail/{productId}")]
        public IActionResult ProductDetail(int productId)
        {
            ProductService Service = new ProductService(_ctx);
            ProductDetail product = Service.GetProductDetail(productId);

            return Ok(product);
        }

        [HttpGet("RequestDetail/{requestId}")]
        public IActionResult RequestDetail(int requestId)
        {
            RequestClass Service = new RequestClass(_ctx);
            RequestDetail requestDetail = Service.GetRequestDetail(requestId);
            return Ok(requestDetail);
        }


        [HttpGet("BrandDetail/{brandId}")]
        public IActionResult BrandDetail(int brandId)
        {
            //BrandService Service = new BrandService(_ctx);
            //BrandDetail brandDetail = Service.GetBrandDetail(brandId);
            //return Ok(brandDetail);

            var y = _ctx.Products.Where(x => x.BrandId == brandId)
                .SelectMany(y => y.ProductCategory)
                .GroupBy(x => new { CatId = x.Category.Id, CategoryName = x.Category.Name })
                .Select(g => new { g.Key.CatId, g.Key.CategoryName});

            //.Select(z => new CategoryTemp
            //{
            //    CatId = z.Category.Id,
            //    CategoryName = z.Category.Name,
            //}));

            //        var results= persons.GroupBy(n => new { n.PersonId, n.car})
            //.Select(g => new {
            //               g.Key.PersonId,
            //               g.Key.car)}).ToList();


            var brandsProductsCategories = _ctx.Brands
                .Where(x => x.Id == brandId)
                .Select(brand => new 
                {
                    brandname = brand.BrandName,
                    requestnum = brand.Products.Select(x => x.InfoRequests).Count(),
                    listcat0 = y.ToList(),
                    //listcat1 = pc.Distinct().ToList(),
                    //listcat2 = pc2.ToList(),

                    //products = brand.Products.Select(product => new ProductTemp
                    //{
                    //    ProductId=product.Id,
                    //    ProductName = product.Name,
                    //    ProductRequestNumber = product.InfoRequests.Count
                    //})
                }).FirstOrDefault();

            return Ok(brandsProductsCategories);

            #region Categories
            var pc =
                from product in _ctx.Products
                join productCategory in _ctx.ProductCategories on product.Id equals productCategory.IdProduct
                join category in _ctx.Categories on productCategory.IdCategory equals category.Id
                where product.BrandId == brandId
                select new { Id = category.Id, Name = category.Name };

            var pc2 =  from product in _ctx.Set<Product>()
                       join productCategory in _ctx.Set<ProductCategory>() on product.Id equals productCategory.IdProduct
                       join category in _ctx.Set<Category>() on productCategory.IdCategory equals category.Id
                       where product.BrandId == brandId
                       group category by new { category.Id, category.Name } into NewGroup
                       select new { NewGroup.Key , Count = NewGroup.Count() };



            var x = _ctx.Products.Where(x=>x.BrandId == brandId)
                    .SelectMany(y => y.ProductCategory
                        .Select(z => new
                        {
                            Id = z.Category.Id,
                            CategoryName = z.Category.Name,
                            TotalProducts = z.Category.ProductCategory.Select(c => c.Product).Where(v => v.BrandId == brandId).Count(),
                        }))
                .Distinct().ToList();
            #endregion
            #region tests
            //var brandsProductsCategories = _ctx.Brands
            //    .Include(x => x.Products)
            //        .ThenInclude(x => x.ProductCategory)
            //            .ThenInclude(x => x.Category)
            //    .Where(x => x.Id == brandId)
            //    .Select(brand => new
            //    {
            //        brandname = brand.BrandName,
            //        requestnum = brand.Products.Select(x => x.InfoRequests).Count(),
            //        listcat0 = x,
            //        listcat1 = pc.Distinct().ToList(),
            //        listcat2 = pc2.ToList(),

            //        products = brand.Products.Select(product => new
            //        {
            //            product.Id,
            //            product.Name,
            //            product.InfoRequests.Count
            //        })

            //    }).ToList();


            //return Ok(brandsProductsCategories);

            //return Ok(p);

            //var result = hotels.SelectMany(
            //            hotel => hotel.RoomType
            //            .Select(room => new { Id = room.RoomId, Hotel = hotel }))
            //       .GroupBy(item => item.Id)
            //       .Select(group => group.FirstOrDefault());
            #endregion
        }


        #region tests
        [HttpGet("brandtest/{id}")]
        public IActionResult GetBrandsProductsCategories(int id)
        {
            var brandsProductsCategories = _ctx.Brands
               .Include(x => x.Products)
                    .ThenInclude(y => y.ProductCategory)
                        .ThenInclude(c => c.Category)
                .Where(x => x.Id == id)
                .Select(brand => new
                {
                    BrandName = brand.BrandName,
                    Products = brand.Products.Select(product => new
                    {
                        product.Id,
                        product.Name,
                        Categories = product.ProductCategory.Select(pc => new
                        {
                            Category = pc.IdCategory
                        })
                    })

                })
                .ToList();
            return Ok(brandsProductsCategories);
        }
        #endregion
    }










    public static class DistinctHelper
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var identifiedKeys = new HashSet<TKey>();
            return source.Where(element => identifiedKeys.Add(keySelector(element)));
        }
    }
}
