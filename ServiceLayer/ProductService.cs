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
    public class ProductService //: IServiceGeneral<ProductSelect,ProductDetail,Product>
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
        public async Task<EntityPage<ProductSelect>> GetProductPageAsync(int pageNum, int pageSize, int orderBy, bool isAsc, int? brandId)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize must be > 0");
            if (pageNum <= 0)
                throw new ArgumentOutOfRangeException("pageNum must be > 0");

            EntityPage<ProductSelect> page = new EntityPage<ProductSelect>();

            page.PageSize = pageSize;
            page.TotalEntitiesNumber = _productRepo.GetProductsNumber();
            page.CurrentPageNumber = pageNum;
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);

            var products = _productRepo.GetAll();
            if(! (brandId is null))
                products = products.Where(x=> x.BrandId == brandId);

            switch (orderBy)
            {
                case 1:
                    // Brand
                    products =  isAsc? products.OrderBy(x => x.Brand.BrandName): products.OrderByDescending(x => x.Brand.BrandName);
                    break;
                case 2:
                    // Nome
                    products = isAsc ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                    break;
                case 3:
                    //Prezzo
                    products = isAsc ? products.OrderBy(x=>x.Price) : products.OrderByDescending(x => x.Price);
                    break;
                default:
                    // Brand + Nome
                    products.OrderBy(x => x.Brand.BrandName).ThenBy(x=> x.Name);
                    break;
            }

            var x = await  products
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(p => new ProductSelect {
                    Id = p.Id, 
                    ProductName = p.Name, 
                    Description = p.ShortDescription,
                    Categories = p.ProductCategory.Select(x => x.Category.Name),
                    BrandName = p.Brand.BrandName,
                })
                .ToListAsync();
            page.ListEntities = x;

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
