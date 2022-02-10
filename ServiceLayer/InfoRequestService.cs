﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepoLayer;

namespace ServiceLayer
{
    public class InfoRequestService
    {
        private RequestRepo _requestRepo;

        public InfoRequestService(RequestRepo requestRepo)
        {
            _requestRepo = requestRepo;
        }

        public Task<IEnumerable<InfoRequest>> GetAll()
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// get a page of Requests
        /// </summary>
        /// <param name="pageNum">number of the page. must be greater than 0</param>
        /// <param name="pageSize">size of the page. must be greater than 0</param>
        /// <returns>An EntityPage object with the info relative to the paging and the list of Requests</returns>
        /// <exception cref="ArgumentOutOfRangeException"> if pagenum and pagesize less than 1 throw exception</exception>
        public async Task<EntityPage<RequestSelect>> GetPageAsync(int pageNum, int pageSize, int searchByProductId, int searchByBrandId,bool Asc)
        {
            if (pageNum < 1 || pageSize < 1)
                throw new ArgumentOutOfRangeException("pagenumber and pagesize must be greater than 0");

            EntityPage<RequestSelect> page = new EntityPage<RequestSelect>();
            page.CurrentPageNumber = pageNum;
            page.PageSize = pageSize;
            page.TotalEntitiesNumber = _requestRepo.GetNumber();
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);


            var requests = _requestRepo.GetAll();
            if (searchByBrandId !=0)
                requests = requests.Where(x => x.Product.BrandId == searchByBrandId);
            if (searchByProductId !=0)
                requests = requests.Where(x => x.Product.Id == searchByProductId);


            requests = Asc? requests.OrderBy(x => x.InsertDate): requests.OrderByDescending(x=>x.InsertDate);
            page.ListEntities = await requests
               .Skip((pageNum - 1) * pageSize).Take(pageSize)
               .Select(r => new RequestSelect
               {
                   Id = r.Id,
                   UserName = r.Name + " " + r.LastName,
                   ProductName = r.Product.Name,
                   PhoneNumber = r.PhoneNumber,
                   Email = r.Email,
                   RequestText = r.RequestText,
                   Date = r.InsertDate,
               })
               .ToListAsync();
            return page;
        }

        /// <summary>
        /// Fetch the details of a specific Request given the id.
        /// that includes the replies of that request, the infos of the product in the request
        /// and the info of the user who made the request
        /// </summary>
        /// <param name="requestId">id of the request to fetch</param>
        /// <returns>a projection model with the requested infos</returns>
        public async Task<RequestDetail> GetRequestDetailAsync(int requestId)
        {
            if (requestId < 1)
                throw new ArgumentOutOfRangeException("requestId id must be > 0");

            RequestDetail inforequest = await _requestRepo.GetAll().Where(r => r.Id == requestId)
                .Select(r => new RequestDetail
                {
                    RequestId = r.Id,
                    RequestText = r.RequestText,
                    ProductId = r.ProductId,
                    ProductName = r.Product.Name,
                    BrandName = r.Product.Brand.BrandName,
                    UserFullName = r.Name + " " + r.LastName,
                    Email = r.Email,
                    InfoUser = r.City + " " + r.Cap + " " + r.Nation.Name,

                    Replies = r.InfoRequestReplies.Select(ir => new RepliesTemp
                    {
                        ReplyId = ir.Id,
                        Date = ir.InsertDate,
                        AccountName = ir.Account.AccountType == 1 ? "User" + ir.Account.User.Name : " Brand"+ ir.Account.Brand.BrandName,
                        ReplyText = ir.ReplyText
                    })
                }).FirstOrDefaultAsync();

            return inforequest;
        }





    }

    public class RequestSelect
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RequestText { get; set; }
        public DateTime Date { get; set; }
    }
}
