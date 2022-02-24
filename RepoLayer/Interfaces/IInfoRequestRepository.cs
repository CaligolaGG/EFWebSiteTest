using Domain;
using System.Linq;

namespace RepoLayer
{
    public interface IInfoRequestRepository
    {
        IQueryable<InfoRequest> GetAll();
        int GetNumber();
    }
}