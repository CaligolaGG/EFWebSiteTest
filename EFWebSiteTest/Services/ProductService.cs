

using System;
using System.Collections.Generic;
using System.Linq;

namespace EFWebSiteTest
{
    public class ProductService
    {
        private MyDbContext _ctx;

        public ProductService(MyDbContext ctx) 
        {
            _ctx = ctx;
        }

        public EntityPage<ProductSelect> GetProductPage(int pageNum, int pagesize)
        {
            EntityPage<ProductSelect> productPageTemp = new EntityPage<ProductSelect>();

            productPageTemp.Entities = _ctx.Products
            .Skip(pageNum * pagesize).Take(pagesize)
            .Select(p => new ProductSelect { Id = p.Id, ProductName = p.Name, Description = p.ShortDescription })
            .ToList();

            productPageTemp.NumberEntities = _ctx.Products.Count();
            productPageTemp.PageNum = pageNum;
            productPageTemp.PageSize = pagesize;

            return productPageTemp;
        }

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

}


//var product = _ctx.Products.Where(p => p.Id == productId)
//    .Select( p=> new {
//        p.Id,
//        p.Name,
//        p.Brand.BrandName,
//        categories = p.ProductCategory.Select(pc=> new 
//        { 
//            pc.IdCategory,
//            pc.Category.Name
//        }), 
//        guestNumberRequest = p.InfoRequests.Where(u=>u.UserId ==null).Count(),
//        loggedNumberrequest = p.InfoRequests.Where(u=> u.UserId != null).Count(),
//        Requests = p.InfoRequests.Select(ir => new { 
//            ir.Id,
//            fullname = ir.UserId == null? ir.Name +" "+ir.LastName: ir.User.Name +" "+ ir.User.LastName + " LOGGED",
//            ir.InfoRequestReplies.Count,
//            lastReply = ir.InfoRequestReplies.OrderByDescending(r=>r.InsertDate).Select(reply=>reply.InsertDate).FirstOrDefault()
//        })
//    });
