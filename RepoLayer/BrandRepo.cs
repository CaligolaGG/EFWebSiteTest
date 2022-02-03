using Microsoft.EntityFrameworkCore;
using RepoLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace RepoLayer
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

        public int GetBrandNumber() => _ctx.Brands.Count();
        public IQueryable<Brand> GetAll() =>  _ctx.Brands;
        public IQueryable<Brand> GetById(int Id) => _ctx.Brands.Where(brand=> brand.Id == Id);

            

        /// <summary>
        /// Fetch the details of a specific brand given the id.
        /// That includes list of the products relative to the brand
        /// and the list of categories of the products of the brand
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns>a BrandDetail object that holds the requested info</returns>
        public async Task<BrandDetail> GetBrandDetailAsync(int brandId)
        {
            if (brandId < 1)
                throw new ArgumentOutOfRangeException("brand id must be > 0");

            #region Categories
            var pc =
                from product in _ctx.Products
                join productCategory in _ctx.ProductCategories on product.Id equals productCategory.IdProduct
                join category in _ctx.Categories on productCategory.IdCategory equals category.Id
                where product.BrandId == brandId
                select new CategoryTemp { CategoryId = category.Id, CategoryName = category.Name , 
                    TotalProducts = category.ProductCategory.Where(p=>p.Product.BrandId == brandId).Count()};

            var pc2 = from product in _ctx.Set<Product>()
                      join productCategory in _ctx.Set<ProductCategory>() on product.Id equals productCategory.IdProduct
                      join category in _ctx.Set<Category>() on productCategory.IdCategory equals category.Id
                      where product.BrandId == brandId
                      group category by new { category.Id, category.Name } into NewGroup
                      select new CategoryTemp { CategoryId = NewGroup.Key.Id, CategoryName = NewGroup.Key.Name,
                          TotalProducts = NewGroup.Count() };

            var x = _ctx.Products.Where(x => x.BrandId == brandId)
                    .SelectMany(y => y.ProductCategory
                        .Select(z => new CategoryTemp
                        {
                            CategoryId = z.Category.Id,
                            CategoryName = z.Category.Name,
                            TotalProducts = z.Category.ProductCategory.Select(c => c.Product).Where(v => v.BrandId == brandId).Count(),
                        }))
                    .Distinct().ToList();

            var categories1 = _ctx.Products.Where(x => x.BrandId == brandId)
                .SelectMany(y => y.ProductCategory)
                .GroupBy(x => new { CatId = x.Category.Id, CategoryName = x.Category.Name })
                .Select(g => new CategoryTemp { CategoryId= g.Key.CatId, CategoryName= g.Key.CategoryName, TotalProducts = g.Count()});

            #endregion

            BrandDetail brandsProductsCategories = await _ctx.Brands
                .Where(x => x.Id == brandId)
                .Select(brand => new BrandDetail
                {
                    BrandName = brand.BrandName,
                    NumberRequests = brand.Products.SelectMany(x => x.InfoRequests).Count(),

                    ListCategories = categories1.ToList(),

                    ListProducts = brand.Products.Select(product => new ProductTemp
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductRequestNumber = product.InfoRequests.Count
                    })
                }).FirstOrDefaultAsync();

            return brandsProductsCategories;
        }


    }

    #region ProjectionModels

    /// <summary>
    /// class that holds detail of a brand
    /// </summary>
    public class BrandDetail 
    {
        /// <summary>
        /// Name of the brand searched for
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// number of the requests for the products of the brand
        /// </summary>
        public int NumberRequests { get; set; }
        /// <summary>
        /// list of categories of the brand,with the number of the brand's products that belong to that category
        /// </summary>
        public IEnumerable<CategoryTemp> ListCategories { get; set; }

        /// <summary>
        /// list of product(projections) related to the brand
        /// </summary>
        public IEnumerable<ProductTemp> ListProducts { get; set; }
    }

    /// <summary>
    /// Class for the BrandDetail class, with the category properties and 
    /// the number of the brand's products that belong to that category
    /// </summary>
    public class CategoryTemp 
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalProducts { get; set; }
    }

    /// <summary>
    /// projection class of product for the BrandDetail class
    /// </summary>
    public class ProductTemp 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductRequestNumber { get; set; }
    }

    /// <summary>
    /// projection class for the brands paging method
    /// </summary>
    public class BrandSelect
    {
        public int BrandId { get; set; } 
        public string BrandName { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> ProductIds { get; set; }
    }
    #endregion
}
