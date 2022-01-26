using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using RepoLayer;

namespace ServiceLayer
{
    public class InfoRequestService : IServiceGeneral<RequestDetail, RequestDetail>
    {
        private RequestRepo _requestRepo;

        public InfoRequestService(RequestRepo requestRepo) 
        {
            _requestRepo = requestRepo;
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

        public Task<EntityPage<RequestDetail>> GetPageAsync(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }


    }
}
