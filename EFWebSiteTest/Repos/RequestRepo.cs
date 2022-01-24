using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EFWebSiteTest
{
    /// <summary>
    /// Class to interact with the InfoRequest table in the db
    /// </summary>
    public class RequestRepo
    {
        private MyDbContext _ctx;

        public RequestRepo(MyDbContext ctx)
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

    #region TempModels
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
    #endregion
}

