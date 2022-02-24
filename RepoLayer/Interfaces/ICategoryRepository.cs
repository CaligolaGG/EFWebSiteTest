using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoLayer
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}