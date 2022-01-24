using System;
using System.Collections.Generic;
using System.Linq;

namespace EFWebSiteTest
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

        /// <summary>
        /// Returns a page of Products
        /// </summary>
        /// <param name="pageNum">number of the page, must be positive, page starts from 1</param>
        /// <param name="pagesize">size of the page,  must be positive</param>
        public EntityPage<ProductSelect> GetProductPage(int pageNum, int pagesize)
        {
            if (pagesize <= 0)
                throw new ArgumentOutOfRangeException("pageSize must be > 0");
            if(pageNum <= 0)
                throw new ArgumentOutOfRangeException("pageNum must be > 0");

            EntityPage<ProductSelect> productPageTemp = new EntityPage<ProductSelect>();

            productPageTemp.Entities = _ctx.Products
            .Skip( (pageNum-1) * pagesize).Take(pagesize)
            .Select(p => new ProductSelect { Id = p.Id, ProductName = p.Name, Description = p.ShortDescription })
            .ToList();

            productPageTemp.NumberEntities = _ctx.Products.Count();
            productPageTemp.PageNum = pageNum;
            productPageTemp.PageSize = pagesize;

            return productPageTemp;
        }

        /// <summary>
        /// returns the detail of a product.
        /// That includes the categories of the product and the requests relative to the product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>a ProductDetail object, that hold the requested infos</returns>
        public ProductDetail GetProductDetail(int productId) 
        {
            ProductDetail product = _ctx.Products.Where(p => p.Id == productId)
                .Select(p => new ProductDetail
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    BrandName = p.Brand.BrandName,
                    guestNumberRequest = p.InfoRequests.Where(u => u.UserId == null).Count(),
                    loggedNumberRequest = p.InfoRequests.Where(u => u.UserId != null).Count(),
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
                }).FirstOrDefault();

            return product;
        }
    }

    #region TempModels
    public class ProductDetail 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public int guestNumberRequest { get; set; }
        public int loggedNumberRequest { get; set; }
        
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<InfoRequestTemp> Requests { get; set; }
    }

    public class InfoRequestTemp 
    { 
        public int RequestId { get; set; }
        public string FullName { get; set; }
        public int RepliesCount { get; set; }
        public DateTime LastReply { get; set; }
    }

    public class ProductSelect
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
    }
    #endregion
}

