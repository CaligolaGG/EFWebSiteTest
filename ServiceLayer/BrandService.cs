using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepoLayer;

namespace ServiceLayer
{
    /// <summary>
    /// Service that uses the BrandRepo for actions relative to the Brand table of the db
    /// </summary>
    public class BrandService
    {
        private readonly BrandRepo _brandRepo;
        public BrandService(BrandRepo brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync() => await _brandRepo.GetAll().ToListAsync();


        /// <summary>
        /// Fetch the details of a specific brand given the id.
        /// That includes list of the products relative to the brand
        /// and the list of categories of the products of the brand
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns>a BrandDetail object that holds the requested info</returns>
        public async Task<BrandDetail> GetDetailAsync(int brandId)
        {
            if (brandId < 1)
                throw new ArgumentOutOfRangeException("brand id must be > 0");


            var categories1 = _brandRepo.GetRelativeCategories(brandId);

            BrandDetail brandsProductsCategories = await _brandRepo.GetAll()
                .Where(x => x.Id == brandId)
                .Select(brand => new BrandDetail
                {
                    BrandName = brand.BrandName,
                    NumberRequests = brand.Products.SelectMany(x => x.InfoRequests).Count(),

                    ListCategories = categories1,

                    ListProducts = brand.Products.Select(product => new ProductTemp
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductRequestNumber = product.InfoRequests.Count
                    })
                }).FirstOrDefaultAsync();

            return brandsProductsCategories;
        }


        /// <summary>
        /// get a page of brands
        /// </summary>
        /// <param name="pageNum">number of the page. must be greater than 0</param>
        /// <param name="pageSize">size of the page. must be greater than 0</param>
        /// <returns>An EntityPage object with the info relative to the paging and the list of brands</returns>
        /// <exception cref="ArgumentOutOfRangeException"> if pagenum and pagesize less than 1 throw exception</exception>
        public async Task<EntityPage<BrandSelect>> GetPageAsync(int pageNum, int pageSize, string searchByName = "")
        {
            if (pageNum < 1 || pageSize < 1)
                throw new ArgumentOutOfRangeException("pagenumber and pagesize must be greater than 0");
            
            EntityPage<BrandSelect> page = new EntityPage<BrandSelect>();
            page.CurrentPageNumber = pageNum;
            page.PageSize = pageSize;
            page.TotalEntitiesNumber = _brandRepo.GetBrandNumber();
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);

            var brands = _brandRepo.GetAll();
            if (!(searchByName is null || searchByName == ""))
                brands = brands.Where(brand => brand.BrandName == searchByName);
            IQueryable<BrandSelect> result = brands.OrderBy(x => x.BrandName)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(brand => new BrandSelect
                {
                    BrandId = brand.Id,
                    BrandName = brand.BrandName,
                    Description = brand.Description,
                    ProductIds = brand.Products.Select(product => product.Id)
                });
            page.ListEntities = await result.ToListAsync();
            
            return page;
        }

        /// <summary>
        /// Update a brand informations
        /// </summary>
        /// <param name="brand">brand to update</param>
        /// <returns>Number of records affected</returns>
        /// <exception cref="ArgumentException">Raise an exception if the inserted brand is null or his id is less than 1</exception>
        public async Task<int> BrandUpdateAsync(Brand brand)
        {
            if (brand.Id < 0 || brand is null || String.IsNullOrEmpty(brand.BrandName))
                throw new ArgumentException("invalid brand");
            return await _brandRepo.UpdateBrandAsync(brand);
        }

        public async Task<int> BrandDeleteLogicalAsync(int brandId)
        {
            if (brandId < 0 )
                throw new ArgumentException("invalid brandId");
            return await _brandRepo.LogicalBrandDeleteAsync(brandId);

        }

    }
}