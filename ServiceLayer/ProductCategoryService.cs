using RepoLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServiceLayer
{
    public class ProductCategoryService
    {
        private ProductCategoryRepo _productCategoryRepo;
        public ProductCategoryService(ProductCategoryRepo productCategoryrepo)
        {
            _productCategoryRepo = productCategoryrepo;
        }

        /// <summary>
        /// Insert a new product with the associated categories
        /// </summary>
        /// <returns>number of records added</returns>
        /// <exception cref="ArgumentException">Raised if the product is null</exception>
        public async Task<int> InsertMultiple(List<ProductCategory> productCategories) 
        {
            if (productCategories is null || productCategories.Count < 1 )
                throw new ArgumentException(nameof(productCategories));

            return await _productCategoryRepo.CreateMultipleAsync(productCategories);
        }
    }
}
