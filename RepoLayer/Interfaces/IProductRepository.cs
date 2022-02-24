using Domain;
using System.Linq;
using System.Threading.Tasks;

namespace RepoLayer
{
    public interface IProductRepository
    {
        Task<int> CreateOrUpdateAsync(ProductAndCategoryModel model);
        Task<int> DeleteLogicalAsync(int productId);
        IQueryable<Product> GetAll();
    }
}