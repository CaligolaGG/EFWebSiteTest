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

        public async Task<int> UpdateBrandAsync(Brand brand)
        {
            _ctx.Brands.Update(brand);
            return await _ctx.SaveChangesAsync();
        }

        public List<CategoryTemp> GetRelativeCategories(int brandId)
        {
            #region other methods
            var pc2 = from product in _ctx.Set<Product>()
                      join productCategory in _ctx.Set<ProductCategory>() on product.Id equals productCategory.IdProduct
                      join category in _ctx.Set<Category>() on productCategory.IdCategory equals category.Id
                      where product.BrandId == brandId
                      group category by new { category.Id, category.Name } into NewGroup
                      select new CategoryTemp
                      {
                          CategoryId = NewGroup.Key.Id,
                          CategoryName = NewGroup.Key.Name,
                          TotalProducts = NewGroup.Count()
                      };

            var x = _ctx.Products.Where(x => x.BrandId == brandId)
                    .SelectMany(y => y.ProductCategory
                        .Select(z => new CategoryTemp
                        {
                            CategoryId = z.Category.Id,
                            CategoryName = z.Category.Name,
                            TotalProducts = z.Category.ProductCategory.Select(c => c.Product).Where(v => v.BrandId == brandId).Count(),
                        }))
                    .Distinct().ToList();


            var pc =
                from product in _ctx.Products
                join productCategory in _ctx.ProductCategories on product.Id equals productCategory.IdProduct
                join category in _ctx.Categories on productCategory.IdCategory equals category.Id
                where product.BrandId == brandId
                select new CategoryTemp
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    TotalProducts = category.ProductCategory.Where(p => p.Product.BrandId == brandId).Count()
                };
            #endregion

            List<CategoryTemp> categories1 = _ctx.Products.Where(x => x.BrandId == brandId)
                .SelectMany(y => y.ProductCategory)
                .GroupBy(x => new { CatId = x.Category.Id, CategoryName = x.Category.Name })
                .Select(g => new CategoryTemp { CategoryId = g.Key.CatId, CategoryName = g.Key.CategoryName, TotalProducts = g.Count() }).ToList();
            return categories1;
        }

        public async Task<int> LogicalBrandDeleteAsync(int brandId)
        {
            Brand brand = _ctx.Brands.Where(x => x.Id == brandId).FirstOrDefault();
            brand.IsDeleted = true;
            //TODO
            _ctx.Brands.Update(brand);
            return await _ctx.SaveChangesAsync();
        }
    }
}
