using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;
using Z.EntityFramework.Plus;
using Microsoft.EntityFrameworkCore.Storage;

namespace RepoLayer
{
    /// <summary>
    /// Class to interact with the Product table in the db
    /// </summary>
    public class ProductRepo
    {
        private MyDbContext _ctx;
        public ProductRepo(MyDbContext ctx) 
        {
            _ctx = ctx;
        }

        public IQueryable<Product> GetAll() => _ctx.Products;

        /// <summary>
        /// Insert or update a new product. 
        /// Add the product if the product id is 0 (not set). Update if the id is != 0
        /// </summary>
        /// <param name="product">product to insert or change</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> CreateOrUpdateAsync(Product product)
        {
            if (product.Id != 0)
                _ctx.Products.Update(product);
            else
                await _ctx.Products.AddAsync(product);
             await _ctx.SaveChangesAsync();
            return product.Id;
        }


        /// <summary>
        /// logically deletes a product with all the request associated with that product and all the replies to that request
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Number of records affected</returns>
        public async Task<int> DeleteLogicalAsync(int productId)
        {
            IDbContextTransaction transaction = _ctx.Database.BeginTransaction();

            try
            { 
                Product product = _ctx.Products.Where(x => x.Id == productId).FirstOrDefault();
                product.IsDeleted = true;
                _ctx.Products.Update(product);

            
                await _ctx.InfoRequestReplies.Where(x => x.InfoRequest.ProductId == productId)
                .UpdateFromQueryAsync(x => new InfoRequestReply() { IsDeleted = true });

                await _ctx.InfoRequests.Where(x => x.ProductId == productId)
                .UpdateFromQueryAsync(x => new InfoRequest() { IsDeleted = true });

                int z = await _ctx.SaveChangesAsync();
                transaction.Commit();
                return z;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }


            return 0;

        }





        /// <summary>
        /// #NOTUSED Phisically delete an item from the table given its id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>number of records affected</returns>
        public async Task<int> DeleteAsync(int productId)
        {
            _ctx.Products.Remove(new Product { Id = productId });
            return await _ctx.SaveChangesAsync();
        }

       


    }

}

