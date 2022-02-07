using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;


namespace RepoLayer
{
    public class ProductCategoryRepo
    {
        private MyDbContext _ctx;
        public ProductCategoryRepo(MyDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Insert in the table multiple ProductCategory records
        /// </summary>
        /// <param name="productCategories">List of productCategories</param>
        /// <returns>Number of rows that have been affected</returns>
        public async Task<int> CreateMultipleAsync(List<ProductCategory> productCategories)
        {
            await _ctx.ProductCategories.AddRangeAsync (productCategories);
            return await _ctx.SaveChangesAsync();
        }
    }
}
