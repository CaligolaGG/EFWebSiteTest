using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;

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
            return await _ctx.SaveChangesAsync();
        }

     

        /// <summary>
        /// delete an item from the table given its id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>number of records affected</returns>
        public async Task<int> DeleteAsync(int productId) 
        {
            _ctx.Products.Remove(new Product { Id = productId });
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> DeleteLogicalAsync(int productId)
        {
            Product product =_ctx.Products.Where(x=>x.Id == productId).FirstOrDefault();
            if (!(product is null))
            {
                int x= await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequestReply
                    SET InfoRequestReply.IsDeleted=1
                    FROM InfoRequestReply as reply
                    join InfoRequest as request On reply.InfoRequestId=request.Id
                    join Product as p On request.ProductId=p.Id
                    WHERE p.Id=" + productId);

                int y= await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequest
                    SET InfoRequest.IsDeleted=1
                    FROM InfoRequest as request join Product as p On request.ProductId=p.Id
                    WHERE p.Id=" + productId);

                return x+y;
            }
            return 0;

        }
    }

}

