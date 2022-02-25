using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IProductCategoryService
    {
        Task<int> InsertMultiple(List<ProductCategory> productCategories);
        Task<int> UpdateMultiple(List<ProductCategory> productCategories);
    }
}