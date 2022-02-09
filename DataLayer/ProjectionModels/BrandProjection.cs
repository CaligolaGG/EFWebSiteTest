using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BrandProjection
    {

    }
    public class BrandWithProducts
    {
        public Brand Brand { get; set; }
        public Account Account { get; set; }
        public List<ProductAndCategoryModel> ProductsCategs { get; set; }
    }

    public class BrandProjectionBasic 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    #region ProjectionModels

    /// <summary>
    /// class that holds detail of a brand
    /// </summary>
    public class BrandDetail
    {
        /// <summary>
        /// Name of the brand searched for
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// number of the requests for the products of the brand
        /// </summary>
        public int NumberRequests { get; set; }
        /// <summary>
        /// list of categories of the brand,with the number of the brand's products that belong to that category
        /// </summary>
        public IEnumerable<CategoryTemp> ListCategories { get; set; }

        /// <summary>
        /// list of product(projections) related to the brand
        /// </summary>
        public IEnumerable<ProductTemp> ListProducts { get; set; }
    }

    /// <summary>
    /// Class for the BrandDetail class, with the category properties and 
    /// the number of the brand's products that belong to that category
    /// </summary>
    public class CategoryTemp
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalProducts { get; set; }
    }

    /// <summary>
    /// projection class of product for the BrandDetail class
    /// </summary>
    public class ProductTemp
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductRequestNumber { get; set; }
    }

    /// <summary>
    /// projection class for the brands paging method
    /// </summary>
    public class BrandSelect
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> ProductIds { get; set; }
    }
    #endregion
}
