using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoLayer
{
    public interface IProductCategoryRepository
    {
        Task<int> CreateMultipleAsync(List<ProductCategory> productCategories);
        Task<int> DeleteMultipleAsync(int productId);
    }
}