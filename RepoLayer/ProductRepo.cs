using RepoLayer;
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


        public int GetProductsNumber() => _ctx.Products.Count();
        public IQueryable<Product> GetAll() => _ctx.Products;

       


        /// <summary>
        /// Fetch the detail of a product.
        /// That includes the categories of the product and the requests relative to the product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>a ProductDetail object, that hold the requested infos</returns>
        public async Task<ProductDetail> GetProductDetailAsync(int productId) 
        {
            if (productId < 1)
                throw new ArgumentOutOfRangeException("product id must be > 0");

            ProductDetail product = await _ctx.Products.Where(p => p.Id == productId)
                .Select(p => new ProductDetail
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    BrandName = p.Brand.BrandName,
                    GuestUsersRequestsNumber = p.InfoRequests.Where(u => u.UserId == null).Count(),
                    LoggedUsersRequestsNumber = p.InfoRequests.Where(u => u.UserId != null).Count(),
                    Categories =  p.ProductCategory.Select(pc => new Category
                    {
                        Id = pc.IdCategory,
                        Name = pc.Category.Name
                    }),
                    Requests = p.InfoRequests.Select(ir => new InfoRequestTemp {
                        RequestId=ir.Id,
                        FullName = ir.UserId == null ? ir.Name + " " + ir.LastName : ir.User.Name + " " + ir.User.LastName + " LOGGED",
                        RepliesCount = ir.InfoRequestReplies.Count,
                        LastReply = ir.InfoRequestReplies.OrderByDescending(r => r.InsertDate).Select(reply => reply.InsertDate).FirstOrDefault()
                    })
                }).OrderBy(x=>x.ProductName).FirstOrDefaultAsync();

            return product;
        }
    }

    #region ProjectionModels
    /// <summary>
    /// projection class that hold details of a product
    /// </summary>
    public class ProductDetail 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public int GuestUsersRequestsNumber { get; set; }
        public int LoggedUsersRequestsNumber { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        /// <summary>
        /// list of projection of InfoRequests.
        /// </summary>
        public IEnumerable<InfoRequestTemp> Requests { get; set; }
    }

    /// <summary>
    /// projection class of infoRequest  for the ProductDetail class.
    /// Hold the id of the info request, name of the person who made the request, 
    /// number of replies to that request and the date of the last reply.
    /// </summary>
    public class InfoRequestTemp 
    { 
        public int RequestId { get; set; }
        public string FullName { get; set; }
        public int RepliesCount { get; set; }
        public DateTime LastReply { get; set; }
    }

    /// <summary>
    ///  projection class of Product  for the ProductDetail class
    /// </summary>
    public class ProductSelect
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
    #endregion
}

