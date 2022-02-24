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
        private IProductCategoryRepository _productCategoryRepo;
        public ProductCategoryService(IProductCategoryRepository productCategoryrepo)
        {
            _productCategoryRepo = productCategoryrepo;
        }

        /// <summary>
        /// Insert a list of product category objects
        /// </summary>
        /// <returns>number of records added</returns>
        /// <exception cref="ArgumentException">Raised if the list is empty or null</exception>
        public async Task<int> InsertMultiple(List<ProductCategory> productCategories) 
        {
            if (productCategories is null || productCategories.Count < 1 )
                throw new ArgumentException(nameof(productCategories));

            return await _productCategoryRepo.CreateMultipleAsync(productCategories);
        }


        /// <summary>
        /// Update the categories of a product by eliminating its existing productCategories and inserting the new ones
        /// </summary>
        /// <param name="productCategories"></param>
        /// <returns>number of rows affected</returns>
        /// <exception cref="ArgumentException">Raised if the list is empty or  null</exception>
        public async Task<int> UpdateMultiple(List<ProductCategory> productCategories)
        {
            if (productCategories is null || productCategories.Count < 1)
                throw new ArgumentException(nameof(productCategories));

            await _productCategoryRepo.DeleteMultipleAsync(productCategories.First().IdProduct);
            return await _productCategoryRepo.CreateMultipleAsync(productCategories);
        }
    }
}
