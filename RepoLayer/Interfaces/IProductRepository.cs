using Domain;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateOrUpdateAsync(ProductAndCategoryModel2 model);
        Task<int> DeleteLogicalAsync(int productId);
        IQueryable<Product> GetAll();
    }
}