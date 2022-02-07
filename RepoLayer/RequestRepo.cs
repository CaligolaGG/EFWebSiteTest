using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace RepoLayer
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

        public IQueryable<InfoRequest> GetAll() => _ctx.InfoRequests;

        public int GetNumber() => _ctx.InfoRequests.Count();

        /// <summary>
        /// Fetch the details of a specific Request given the id.
        /// that includes the replies of that request, the infos of the product in the request
        /// and the info of the user who made the request
        /// </summary>
        /// <param name="requestId">id of the request to fetch</param>
        /// <returns></returns>
        public async Task<RequestDetail> GetRequestDetailAsync(int requestId)
        {
            if (requestId < 1)
                throw new ArgumentOutOfRangeException("requestId id must be > 0");

            RequestDetail inforequest = await _ctx.InfoRequests.Where(r => r.Id == requestId)
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
                }).FirstOrDefaultAsync();

            return inforequest;
        }


    }
}

