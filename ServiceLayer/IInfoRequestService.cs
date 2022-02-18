using Domain;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IInfoRequestService
    {
        Task<EntityPage<RequestSelect>> GetPageAsync(int pageNum, int pageSize, string searchByProductName, int searchByBrandId, bool Asc, int searchByProductId);
        Task<RequestDetail> GetRequestDetailAsync(int requestId);
    }
}