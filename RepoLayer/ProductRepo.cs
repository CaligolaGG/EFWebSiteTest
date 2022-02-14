using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;
using Z.EntityFramework.Plus;

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
            Product product = _ctx.Products.Where(x => x.Id == productId).FirstOrDefault();
            product.IsDeleted = true;
            _ctx.Products.Update(product);

            if (!(product is null))
            {
                _ctx.InfoRequests.Where(x => x.ProductId == productId)
                .Update(x => new InfoRequest() { IsDeleted = true });

                
                var temp = await _ctx.InfoRequests.Where(x => x.ProductId == productId).ToListAsync();

                /* TODO info request replies
                var temp =await  _ctx.InfoRequests.Where(x => x.ProductId == productId).ToListAsync();

                _ctx.InfoRequestReplies.
                    Where(x =>temp.Select(s=>s.Id).Contains(x.InfoRequestId))
                    .Update(x => new InfoRequestReply() { IsDeleted = true });   
                */

                int z = await _ctx.SaveChangesAsync();
                return z;
            }
            return 0;

        }


        public async Task<int> CreateProductWithCategories(List<ProductAndCategoryModel> ProductsCategs, int brandId)
        {
            foreach (ProductAndCategoryModel p in ProductsCategs)
            {
                if (String.IsNullOrWhiteSpace(p.Product.Name))
                    throw new Exception("invalid product " + p.Product.Name);
                p.Product.BrandId = brandId;

                foreach (int c in p.Categories)
                {
                    if (c < 1)
                        throw new Exception("invalid category");
                }

                await _ctx.Products.AddAsync(p.Product);
                await _ctx.SaveChangesAsync();

                foreach (int c in p.Categories)
                    await _ctx.ProductCategories.AddAsync(new ProductCategory { IdProduct = p.Product.Id, IdCategory = c });
            }
           return await _ctx.SaveChangesAsync();
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

        /*
        public async Task<int> DeleteLogicalAsync2(int productId)
        {
            Product product = _ctx.Products.Where(x => x.Id == productId).FirstOrDefault();
            product.IsDeleted = true;
            _ctx.Products.Update(product);
            if (!(product is null))
            {
                int x = await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequestReply
                    SET InfoRequestReply.isDeleted=1
                    FROM InfoRequestReply as reply
                    join InfoRequest as request On reply.InfoRequestId=request.Id
                    join Product as p On request.ProductId=p.Id
                    WHERE p.Id=" + productId);

                int y = await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequest
                    SET InfoRequest.isDeleted=1
                    FROM InfoRequest as request join Product as p On request.ProductId=p.Id
                    WHERE p.Id=" + productId);

                int z = await _ctx.SaveChangesAsync();
                return x + y;
            }
            return 0;

        }*/


    }

}

