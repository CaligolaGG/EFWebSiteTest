using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EFWebSiteTest
{
    public class RequestClass
    {

        private MyDbContext _ctx;

        public RequestClass(MyDbContext ctx)
        {
            _ctx = ctx;
        }

        public RequestDetail GetRequestDetail(int requestId)
        {
            RequestDetail inforequest = _ctx.InfoRequests.Where(r => r.Id == requestId)
                .Select(r => new RequestDetail {
                    RequestId = r.Id,
                    ProductId = r.ProductId,
                    ProductName = r.Product.Name,
                    BrandName = r.Product.Brand.BrandName,
                    UserFullName = r.Name + " " + r.LastName,
                    Email=r.Email,
                    InfoUser = r.City + " " + r.Cap + " " + r.Nation.Name,

                    Replies = r.InfoRequestReplies.Select(ir => new RepliesTemp {
                        ReplyId = ir.Id,
                        AccountName = ir.Account.AccountType == 1 ? ir.Account.User.Name : ir.Account.Brand.BrandName + " Brand",
                        ReplyText = ir.ReplyText
                    })
                }).FirstOrDefault();

            return inforequest;
        }

    }

    public class RequestDetail
    {
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string InfoUser { get; set; }

        public IEnumerable<RepliesTemp> Replies { get; set; }
    }

    public class RepliesTemp 
    {
        public int ReplyId { get; set; }
        public string AccountName { get; set; }
        public string ReplyText { get; set; }

    }
}



//var inforequest = _ctx.InfoRequests.Where(r => r.Id == requestId)
//    .Select(r => new {
//        r.Id,
//        ProductId = r.ProductId,
//        ProductName = r.Product.Name,
//        ProductBrandName = r.Product.Brand.BrandName,
//        UserFullName = r.Name + " " + r.LastName,
//        r.Email,
//        infoUser = r.City + " " + r.Cap + " " + r.Nation.Name,

//        Replies = r.InfoRequestReplies.Select(ir => new {
//            ir.Id,
//            Name = ir.Account.AccountType == 1 ?
//            ir.Account.User.Name :
//            ir.Account.Brand.BrandName + " Brand",
//            ir.ReplyText
//        })
//    });