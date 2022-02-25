using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private MyDbContext _ctx;
        public ProductCategoryRepository(MyDbContext ctx)
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
            await _ctx.ProductCategories.AddRangeAsync(productCategories);
            return await _ctx.SaveChangesAsync();
        }


        /// <summary>
        /// Remobe from the table multiple ProductCategory records
        /// </summary>
        /// <param name="productId">id of the product which categories have to be removed</param>
        /// <returns>Number of rows that have been affected</returns>
        public async Task<int> DeleteMultipleAsync(int productId)
        {
            List<ProductCategory> pc = await _ctx.ProductCategories.Where(x => x.IdProduct == productId).ToListAsync();
            _ctx.ProductCategories.RemoveRange(pc);
            return await _ctx.SaveChangesAsync();
        }
    }
}
