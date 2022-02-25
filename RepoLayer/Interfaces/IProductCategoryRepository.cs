using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<int> CreateMultipleAsync(List<ProductCategory> productCategories);
        Task<int> DeleteMultipleAsync(int productId);
    }
}