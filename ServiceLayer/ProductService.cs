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
    public class ProductService 
    {
        private ProductRepo _productRepo;
        public ProductService(ProductRepo productRepo, ProductCategoryRepo pcRepo) 
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
        public async Task<EntityPage<ProductSelect>> GetProductPageAsync(int pageNum, int pageSize, int orderBy, bool isAsc, int brandId =0)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize must be > 0");
            if (pageNum <= 0)
                throw new ArgumentOutOfRangeException("pageNum must be > 0");

            EntityPage<ProductSelect> page = new EntityPage<ProductSelect>();

            page.PageSize = pageSize;
            page.CurrentPageNumber = pageNum;


            var products = _productRepo.GetAll();
            if(brandId > 0)
                products = products.Where(x=> x.BrandId == brandId);
            page.TotalEntitiesNumber = await products.CountAsync();
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);
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
                    products = products.OrderBy(x => x.Brand.BrandName).ThenBy(x=> x.Name);
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
                    Price = p.Price.ToString(), 
                })
                .ToListAsync();
            page.ListEntities = x;

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
        public async Task<int> InsertOrUpdateAsync(Product product) 
        {
            if(product is null || !IsProductValid(product))
                throw new ArgumentNullException(nameof(product));
            if (product.BrandId < 1)
                throw new ArgumentException("product must have a brand");

            return await _productRepo.CreateOrUpdateAsync(product);
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

            ProductDetail product = await _productRepo.GetAll().Where(p => p.Id == productId)
                .Select(p => new ProductDetail
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    BrandId = p.BrandId,
                    BrandName = p.Brand.BrandName,
                    GuestUsersRequestsNumber = p.InfoRequests.Where(u => u.UserId == null).Count(),
                    LoggedUsersRequestsNumber = p.InfoRequests.Where(u => u.UserId != null).Count(),
                    Categories = p.ProductCategory.Select(pc => new Category
                    {
                        Id = pc.IdCategory,
                        Name = pc.Category.Name
                    }),
                    Requests = p.InfoRequests.Select(ir => new InfoRequestTemp
                    {
                        RequestId = ir.Id,
                        FullName = ir.UserId == null ? ir.Name + " " + ir.LastName : ir.User.Name + " " + ir.User.LastName + " LOGGED",
                        RepliesCount = ir.InfoRequestReplies.Count,
                        LastReply = ir.InfoRequestReplies.OrderByDescending(r => r.InsertDate).Select(reply => reply.InsertDate).FirstOrDefault()
                    })
                }).FirstOrDefaultAsync();

            return product;
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
                .Select(p => new ProductAndCategories
                {
                    Product = new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ShortDescription = p.ShortDescription,
                        Price = p.Price,
                        BrandId = p.BrandId,
                    },
                    
                    BrandName = p.Brand.BrandName,
                    Categories = p.ProductCategory.Select(pc => new Category
                    {
                        Id = pc.IdCategory,
                        Name = pc.Category.Name
                    }),

                }).FirstOrDefaultAsync();

           return product;
        }


        /// <summary>
        /// #NOTUSED Phisically Remove a product given an id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>number of records affected</returns>
        /// <exception cref="ArgumentException"> raised if the id is less then 1 </exception>
        public async Task<int> DeleteAsync(int productId)
        {
            if (productId < 1)
                throw new ArgumentException("product id must be > 0");

            return await _productRepo.DeleteAsync(productId);
        }


        private bool IsProductValid(Product product) =>
         product.Name.Length > 0 && product.Name.Length <= 50
         && product.Description.Length <= 50 && product.ShortDescription.Length <= 20
         && product.Price > 0 && product.BrandId > 0;


    }
}
