using Domain;
using System.Linq;

namespace RepositoryLayer.Interfaces
{
    public interface IInfoRequestRepository
    {
        IQueryable<InfoRequest> GetAll();
        int GetNumber();
    }
}