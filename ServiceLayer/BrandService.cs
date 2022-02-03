﻿using System;
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
    public class BrandService //: IServiceGeneral<BrandSelect, BrandDetail,Brand>
    {
        private readonly BrandRepo _brandRepo;
        public BrandService(BrandRepo brandRepo)
        {
            _brandRepo = brandRepo;
        }


        /// <summary>
        /// get the details of the brand
        /// </summary>
        /// <param name="brandId">id of the brand must be greater than 0</param>
        /// <returns>a BrandDetail object  that holds the requested info</returns>
        /// <exception cref="ArgumentOutOfRangeException">brand id must be greater than 0</exception>
        public async Task<BrandDetail> GetDetailAsync(int brandId)
        {
            if (brandId < 1)
                throw new ArgumentOutOfRangeException("brand id must be greater than 0");
            return await _brandRepo.GetBrandDetailAsync(brandId);
        }


        public async Task<IEnumerable<Brand>> GetAllAsync() =>  await _brandRepo.GetAll().ToListAsync();
        


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


    }
}
