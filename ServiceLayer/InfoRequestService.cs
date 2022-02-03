using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepoLayer;

namespace ServiceLayer
{
    public class InfoRequestService //: IServiceGeneral<RequestDetail, RequestDetail, InfoRequest>
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
        /// get the details of the Request through the requestrepo relative function
        /// </summary>
        /// <param name="requestId">id of the request, must be greater than 0</param>
        /// <returns>a RequestDetail object that holds the requested info</returns>
        /// <exception cref="ArgumentOutOfRangeException">request id must be greater than 0</exception>
        public async Task<RequestDetail> GetDetailAsync(int requestId)
        {
            if (requestId < 1)
                throw new ArgumentOutOfRangeException("request id must be greater than 0");
            return await _requestRepo.GetRequestDetailAsync(requestId);
        }

        public async Task<EntityPage<RequestSelect>> GetPageAsync(int pageNum, int pageSize, string searchByProductName, string searchByBrandName)
        {
            if (pageNum < 1 || pageSize < 1)
                throw new ArgumentOutOfRangeException("pagenumber and pagesize must be greater than 0");

            EntityPage<RequestSelect> page = new EntityPage<RequestSelect>();
            page.CurrentPageNumber = pageNum;
            page.PageSize = pageSize;
            page.TotalEntitiesNumber = _requestRepo.GetNumber();
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);


            var requests = _requestRepo.GetAll();
            if (!String.IsNullOrEmpty(searchByBrandName))
                requests = requests.Where(x => x.Product.Name == searchByProductName);
            if (!String.IsNullOrEmpty(searchByBrandName))
                requests = requests.Where(x => x.Product.Brand.BrandName == searchByBrandName);

            page.ListEntities = await requests
               .Skip((pageNum - 1) * pageSize).Take(pageSize)
               .Select(r => new RequestSelect
               {
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


    }

    public class RequestSelect
    {
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RequestText { get; set; }
        public DateTime Date { get; set; }
    }
}
