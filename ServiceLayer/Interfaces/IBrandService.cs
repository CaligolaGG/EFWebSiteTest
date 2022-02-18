using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IBrandService
    {
        Task<int> BrandDeleteLogicalAsync(int brandId);
        Task<Dictionary<string, string>> BrandFieldsValidation(string name, string email);
        Task<int> BrandUpdateAsync(Brand brand);
        Task<int> CreateBrandWithProductsAsync(BrandWithProducts brandWithProducts);
        Task<List<BrandProjectionBasic>> GetAllAsync();
        Task<List<BrandAccountProjection>> GetAllBrandAccountAsync();
        Task<Brand> GetBrandAsync(int brandId);
        Task<BrandDetail> GetDetailAsync(int brandId);
        Task<EntityPage<BrandSelect>> GetPageAsync(int pageNum, int pageSize, string searchByName = "");
    }
}