using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{

    /// <summary>
    /// basic projection of the class Brand. hold name and id
    /// </summary>
    public class BrandProjectionBasic
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
    }

    public class BrandWithProducts
    {
        public Brand Brand { get; set; }
        public Account Account { get; set; }
        public List<ProductAndCategoryModel> ProductsCategs { get; set; }
    }



    /// <summary>
    /// projection of the class brand and account. holds name of brand and account email.Usefull for validation purposes
    /// </summary>
    public class BrandAccountProjection : BrandProjectionBasic
    {
        public string Email { get; set; }
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
    /// projection class for the brands paging method
    /// </summary>
    public class BrandSelect : BrandProjectionBasic
    {
        public string Description { get; set; }
        public IEnumerable<int> ProductIds { get; set; }
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
    #endregion
}
