using RepoLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ServiceLayer
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepo;
        private IMapper _mapper;
        public ProductService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// get a page of Products through the relative Productrepo method
        /// </summary>
        /// <param name="pageNum">number of the page. must be greater than 0</param>
        /// <param name="pageSize">size of the page. must be greater than 0</param>
        /// <returns>An EntityPage object with the info relative to the paging and the list of products</returns>
        /// <exception cref="ArgumentOutOfRangeException"> if pagenum and pagesize less than 1 throw exception</exception>
        public async Task<EntityPage<ProductSelect>> GetProductPageAsync(int pageNum, int pageSize, Order orderBy, bool isAsc, int brandId = 0)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize must be > 0");
            if (pageNum <= 0)
                throw new ArgumentOutOfRangeException("pageNum must be > 0");

            EntityPage<ProductSelect> page = new EntityPage<ProductSelect>();

            page.PageSize = pageSize;
            page.CurrentPageNumber = pageNum;


            var products = _productRepo.GetAll();
            if (brandId > 0)
                products = products.Where(x => x.BrandId == brandId);
            page.TotalEntitiesNumber = await products.CountAsync();
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);
            switch (orderBy)
            {
                case Order.Brand:
                    // Brand
                    products = isAsc ? products.OrderBy(x => x.Brand.BrandName) : products.OrderByDescending(x => x.Brand.BrandName);
                    break;
                case Order.Name:
                    // Nome
                    products = isAsc ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                    break;
                case Order.Price:
                    //Prezzo
                    products = isAsc ? products.OrderBy(x => x.Price) : products.OrderByDescending(x => x.Price);
                    break;
                default:
                    // Brand + Nome
                    products = products.OrderBy(x => x.Brand.BrandName).ThenBy(x => x.Name);
                    break;
            }

            page.ListEntities = await products
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .ProjectTo<ProductSelect>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return page;
        }

        /// <summary>
        /// Update or Insert a new product. 
        /// Insert if the product id is 0 (not set). Update if the id is > 0
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Raised if the product is null</exception>
        /// <exception cref="ArgumentException">Raised if the product doesnt :  have a brand, have a valid name, price is negative </exception>
        public async Task<int> InsertOrUpdateAsync(ProductAndCategoryModel model)
        {
            if (model.Product is null || !IsProductValid(model.Product))
                throw new ArgumentNullException(nameof(model.Product));
            if (model.Product.BrandId < 1)
                throw new ArgumentException("product must have a brand");

            return await _productRepo.CreateOrUpdateAsync(model);
        }


        /// <summary>
        /// Remove logically a product given an id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>number of records affected</returns>
        /// <exception cref="ArgumentException"> raised if the id is less then 1 </exception>
        public async Task<int> DeleteLogicalAsync(int productId)
        {
            if (productId < 1)
                throw new ArgumentException("product id must be > 0");

            return await _productRepo.DeleteLogicalAsync(productId);
        }


        /// <summary>
        /// Fetch the detail of a product.
        /// That includes the categories of the product and the requests relative to the product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>a ProductDetail object, that hold the requested infos</returns>
        public async Task<ProductDetail> GetProductDetailAsync(int productId)
        {
            if (productId < 1)
                throw new ArgumentOutOfRangeException("product id must be > 0");

            Product product = await _productRepo.GetAll().Where(p => p.Id == productId).SingleOrDefaultAsync();

            //ProductDetail product = await _productRepo.GetAll().Where(p => p.Id == productId)
            //    .ProjectTo<ProductDetail>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync()
            //    ;

            ProductDetail productDetail = _mapper.Map<Product,ProductDetail>(product);

  

            return productDetail;
        }

        /// <summary>
        /// Fetch a product with its categories.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>object, that hold the requested infos</returns>
        public async Task<ProductAndCategories> GetProductWithCategoriesAsync(int productId)
        {
            if (productId < 1)
                throw new ArgumentOutOfRangeException("product id must be > 0");

            var product = await _productRepo.GetAll().Where(p => p.Id == productId)
                .ProjectTo<ProductAndCategories>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync();

            return product;
        }

        private bool IsProductValid(Product product) =>
         product.Name.Length > 0 && product.Name.Length <= 50
         && product.Description.Length <= 50 && product.ShortDescription.Length <= 20
         && product.Price > 0;


    }



}
