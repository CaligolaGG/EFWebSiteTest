using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepoLayer;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace ServiceLayer
{
    /// <summary>
    /// Service that uses the BrandRepo for actions relative to the Brand table of the db
    /// </summary>
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// fetch a BrandProjectionBasic object.
        /// </summary>
        /// <returns> a list with id and name of all the brands </returns>
        public async Task<List<BrandProjectionBasic>> GetAllAsync() =>
            await _brandRepo.GetAll().ProjectTo<BrandProjectionBasic>(_mapper.ConfigurationProvider).ToListAsync();

        /// <summary>
        /// fetch a BrandAccountProjection object.
        /// </summary>
        /// <returns>a list with  name and account mail of all the brands</returns>
        public async Task<List<BrandAccountProjection>> GetAllBrandAccountAsync() =>
            await _brandRepo.GetAll()
                .ProjectTo<BrandAccountProjection>(_mapper.ConfigurationProvider)
                .ToListAsync();


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
                .ProjectTo<BrandDetail>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            brandsProductsCategories.ListCategories = categories1.ToList();

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

            var brands = _brandRepo.GetAll();
            if (!(searchByName is null || searchByName == ""))
                brands = brands.Where(brand => brand.BrandName.Contains(searchByName));

            page.TotalEntitiesNumber = brands.Count();
            page.TotalPagesNumber = (int)Math.Ceiling(Convert.ToDecimal(page.TotalEntitiesNumber) / pageSize);

            IQueryable<BrandSelect> result = brands.OrderBy(x => x.BrandName)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .ProjectTo<BrandSelect>(_mapper.ConfigurationProvider);
            page.ListEntities = await result.ToListAsync();

            return page;
        }



        /// <summary>
        /// Logically delete a brand.
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns>Number of db rows affected</returns>
        /// <exception cref="ArgumentException">Raised if the brandId is less than 1</exception>
        public async Task<int> BrandDeleteLogicalAsync(int brandId)
        {
            if (brandId < 1)
                throw new ArgumentException("invalid brandId");
            return await _brandRepo.LogicalBrandDeleteAsync(brandId);
        }

        /// <summary>
        /// Add a new brand with the associated products and their categories
        /// </summary>
        /// <param name="brandWithProducts">Models that contains all the information to pass to the repo</param>
        /// <returns>number of rows affected</returns>
        public async Task<int> CreateBrandWithProductsAsync(BrandWithProducts brandWithProducts)
        {
            if (!IsBrandValid(brandWithProducts.Brand))
                throw new ArgumentException("invalid brand");
            foreach (var p in brandWithProducts.ProductsCategs)
                if (!IsProductValid(p.Product))
                    throw new ArgumentException("found invalid product ");

            int brandId = await _brandRepo.CreateBrandWithProductsAsync(brandWithProducts);

            return brandId;

        }

        /// <summary>
        /// Update a brand informations
        /// </summary>
        /// <param name="brand">brand to update</param>
        /// <returns>Number of records affected</returns>
        /// <exception cref="ArgumentException">Raise an exception if the inserted brand is null or his id is less than 1</exception>
        public async Task<int> BrandUpdateAsync(Brand brand)
        {
            if (!IsBrandValid(brand))
                throw new ArgumentException("invalid brand");
            return await _brandRepo.UpdateBrandAsync(brand);
        }


        /// <summary>
        /// Fetch a specific Brand object given its Id
        /// </summary>
        /// <param name="brandId"></param>
        /// <exception cref="ArgumentException">Raised if brandId is less than 1</exception>
        public async Task<Brand> GetBrandAsync(int brandId)
        {
            if (brandId < 1)
                throw new ArgumentException("brand id must be > 0");
            return await _brandRepo.GetById(brandId).FirstOrDefaultAsync();
        }

        private bool IsBrandValid(Brand brand) => brand.BrandName.Length > 0 && brand.BrandName.Length <= 50 && brand.Description.Length <= 50;
        private bool IsProductValid(Product product) =>
            product.Name.Length > 0 && product.Name.Length <= 50
            && product.Description.Length <= 50 && product.ShortDescription.Length <= 20
            && product.Price > 0;



        public async Task<Dictionary<string, string>> BrandFieldsValidation(string name, string email)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (await _brandRepo.CheckName(name) != 0)
                errors.Add("Name", "brand name already taken");
            if (await _brandRepo.CheckMail(email) != 0)
                errors.Add("Mail", "mail already taken");
            return errors;

        }





    }
}