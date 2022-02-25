using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IBrandRepository
    {
        Task<int> CheckMail(string mail);
        Task<int> CheckName(string name);
        Task<int> CreateBrandWithProductsAsync(BrandWithProducts brandWithProducts);
        IQueryable<Brand> GetAll();
        IQueryable<Brand> GetById(int Id);
        List<CategoryTemp> GetRelativeCategories(int brandId);
        Task<int> LogicalBrandDeleteAsync(int brandId);
        Task<int> UpdateBrandAsync(Brand brand);
    }
}