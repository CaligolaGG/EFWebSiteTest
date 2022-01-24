using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EFWebSiteTest
{
    /// <summary>
    /// Class to interact with the Brand table in the db
    /// </summary>
    public class BrandRepo
    {
        private MyDbContext _ctx;

        public BrandRepo(MyDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Returns a page of Brands with the relative products of the brands
        /// </summary>
        /// <param name="pageNum">number of the page</param>
        /// <param name="pagesize">size of the page</param>
        public EntityPage<BrandSelect> GetBrandPage(int pageNum, int pagesize)
        {
            EntityPage<BrandSelect> brandPageTemp = new EntityPage<BrandSelect>();
            brandPageTemp.Entities =  _ctx.Brands
                .Skip((pageNum - 1) * pagesize).Take(pagesize)
                .Select(brand => new BrandSelect
                    {
                        BrandId = brand.Id,
                        BrandName=brand.BrandName,
                        Description=brand.Description,
                        ProductIds = brand.Products.Select(product =>product.Id )
                    })
                .ToList();
            brandPageTemp.PageNum = pageNum;
            brandPageTemp.PageSize = pagesize;
            brandPageTemp.NumberEntities = _ctx.Brands.Count();

            return brandPageTemp;
        }

        /// <summary>
        /// returns the details of a specific brand given the id.
        /// That includes list of the products relative to the brand
        /// and the list of categories of the products of the brand
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns>a BrandDetail object that holds the requested info</returns>
        public BrandDetail GetBrandDetail(int brandId)
        {
            #region Categories
            var pc =
                from product in _ctx.Products
                join productCategory in _ctx.ProductCategories on product.Id equals productCategory.IdProduct
                join category in _ctx.Categories on productCategory.IdCategory equals category.Id
                where product.BrandId == brandId
                select new CategoryTemp { CatId = category.Id, CategoryName = category.Name , 
                    TotalProducts = category.ProductCategory.Where(p=>p.Product.BrandId == brandId).Count()};

            var pc2 = from product in _ctx.Set<Product>()
                      join productCategory in _ctx.Set<ProductCategory>() on product.Id equals productCategory.IdProduct
                      join category in _ctx.Set<Category>() on productCategory.IdCategory equals category.Id
                      where product.BrandId == brandId
                      group category by new { category.Id, category.Name } into NewGroup
                      select new CategoryTemp { CatId = NewGroup.Key.Id, CategoryName = NewGroup.Key.Name,
                          TotalProducts = NewGroup.Count() };

            var x = _ctx.Products.Where(x => x.BrandId == brandId)
                    .SelectMany(y => y.ProductCategory
                        .Select(z => new CategoryTemp
                        {
                            CatId = z.Category.Id,
                            CategoryName = z.Category.Name,
                            TotalProducts = z.Category.ProductCategory.Select(c => c.Product).Where(v => v.BrandId == brandId).Count(),
                        }))
                    .Distinct().ToList();

            var categories1 = _ctx.Products.Where(x => x.BrandId == brandId)
                .SelectMany(y => y.ProductCategory)
                .GroupBy(x => new { CatId = x.Category.Id, CategoryName = x.Category.Name })
                .Select(g => new CategoryTemp { CatId= g.Key.CatId, CategoryName= g.Key.CategoryName, TotalProducts = g.Count()});

            #endregion

            BrandDetail brandsProductsCategories = _ctx.Brands
                .Where(x => x.Id == brandId)
                .Select(brand => new BrandDetail
                {
                    brandname = brand.BrandName,
                    requestnum = brand.Products.Select(x => x.InfoRequests).Count(),
                    //listCategories = x,
                    //listCategories = pc.Distinct().ToList(),
                    //listCategories = pc2.ToList(),
                    listCategories = categories1.ToList(),

                    products = brand.Products.Select(product => new ProductTemp
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductRequestNumber = product.InfoRequests.Count
                    })
                }).FirstOrDefault();

            return brandsProductsCategories;
        }

    }

    #region TempModels
    public class BrandDetail 
    {
        public string brandname { get; set; }
        public int requestnum { get; set; }
        public IEnumerable<CategoryTemp> listCategories { get; set; }
        public IEnumerable<ProductTemp> products { get; set; }
    }

    public class CategoryTemp 
    {
        public int CatId { get; set; }
        public string CategoryName { get; set; }
        public int TotalProducts { get; set; }
    }

    public class ProductTemp 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductRequestNumber { get; set; }
    }

    public class BrandSelect
    {
        public int BrandId { get; set; } 
        public string BrandName { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> ProductIds { get; set; }
    }
    #endregion
}
