using RepoLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ServiceLayer
{
    public class ProductService : IServiceGeneral<ProductSelect,ProductDetail,Product>
    {
        private ProductRepo _productRepo;
        public ProductService(ProductRepo productRepo) 
        {
            _productRepo = productRepo;
        }

        /// <summary>
        /// get a page of Products through the relative Productrepo method
        /// </summary>
        /// <param name="pageNum">number of the page. must be greater than 0</param>
        /// <param name="pageSize">size of the page. must be greater than 0</param>
        /// <returns>An EntityPage object with the info relative to the paging and the list of products</returns>
        /// <exception cref="ArgumentOutOfRangeException"> if pagenum and pagesize less than 1 throw exception</exception>
        public async Task<EntityPage<ProductSelect>> GetPageAsync(int pageNum, int pageSize) 
        {
            if (pageNum < 1 || pageSize < 1)
                throw new ArgumentOutOfRangeException("pagenumber and pagesize must be greater than 0");
            EntityPage<ProductSelect> page = new EntityPage<ProductSelect>();
            
            page.PageSize = pageSize;
            page.TotalEntitiesNumber = _productRepo.GetProductsNumber();
            page.CurrentPageNumber = pageNum;
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);
            page.ListEntities = await _productRepo.GetProductPageAsync(pageNum,pageSize);

            return page;
        }

        /// <summary>
        /// get the details of the Product through the relative Productrepo method
        /// </summary>
        /// <param name="productId">id of the product, must be greater than 0</param>
        /// <returns>a BrandDetail object  that holds the requested info</returns>
        /// <exception cref="ArgumentOutOfRangeException">product id must be greater than 0</exception>
        public async Task<ProductDetail> GetDetailAsync(int productId) 
        {
            if (productId < 1)
                throw new ArgumentOutOfRangeException("product Id must be greater than 0");
            return await _productRepo.GetProductDetailAsync(productId);
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
