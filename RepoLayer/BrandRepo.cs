using RepoLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Z.EntityFramework.Plus;
using Microsoft.EntityFrameworkCore;



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


        /// <returns> An IQueryable to interrogate the db on all the brands </returns>
        public IQueryable<Brand> GetAll() =>  _ctx.Brands;

        /// <returns> An IQueryable to interrogate the db on a single brand </returns>
        public IQueryable<Brand> GetById(int Id) => _ctx.Brands.Where(brand=> brand.Id == Id);


        /// <summary>
        /// Fetch the categories relative to the brand with id equal to the brandid passed to the method
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns>list of Categories with the number of products in that brand associated</returns>
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

        /// <summary>
        /// Update the fields of a brand 
        /// </summary>
        /// <param name="brand">new brand updated</param>
        /// <returns>number of rows affected</returns>
        public async Task<int> UpdateBrandAsync(Brand brand)
        {
            _ctx.Brands.Update(brand);
            return await _ctx.SaveChangesAsync();
        }


        /// <summary>
        /// Add a new brand with the associated products and their categories
        /// </summary>
        /// <param name="brandWithProducts">Models that contains all the information to add to the db</param>
        /// <returns>number of rows affected</returns>
        public async Task<int> CreateBrandWithProductsAsync(Account account, Brand brand)
        {
            await _ctx.Accounts.AddAsync(account);
            await _ctx.SaveChangesAsync();

            brand.AccountId = account.Id;
            await _ctx.Brands.AddAsync(brand);
            await _ctx.SaveChangesAsync();

            return brand.Id;
        }


        /// <summary>
        /// logically deletes a brand by updating the isDelete value
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns>number of rows changed</returns>
        public async Task<int> LogicalBrandDeleteAsync(int brandId)
        {
            Brand brand = _ctx.Brands.FirstOrDefault(x => x.Id == brandId);
            brand.IsDeleted = true;
            _ctx.Brands.Update(brand);

            _ctx.Products.Where(x => x.BrandId == brandId)
                .Update(x => new Product() { IsDeleted = true });

            /* TODO
            var temp = await _ctx.Products.Where(x => x.BrandId == brandId).ToListAsync();
            _ctx.InfoRequests.Where(x => temp.Select(s => s.Id).Contains(x.ProductId))
                .Update(x => new InfoRequest() { IsDeleted = true });
            */
            return await _ctx.SaveChangesAsync();
        }






        /// <summary>
        /// #NOT USED creates a new brand not yet associated to an account
        /// </summary>
        public async Task<int> CreateBrandAsync(Brand brand)
        {
            await _ctx.Brands.AddAsync(brand);
            return await _ctx.SaveChangesAsync();
        }



    }
}
