using Domain;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IProductService
    {
        Task<int> DeleteLogicalAsync(int productId);
        Task<ProductDetail> GetProductDetailAsync(int productId);
        Task<EntityPage<ProductSelect>> GetProductPageAsync(int pageNum, int pageSize, Order orderBy, bool isAsc, int brandId = 0);
        Task<ProductAndCategories> GetProductWithCategoriesAsync(int productId);
        Task<int> InsertOrUpdateAsync(ProductAndCategoryModel2 model);
    }
}