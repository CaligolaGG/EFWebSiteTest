using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProductProjectionModel
    {
    }
    #region ProjectionModels

    /// <summary>
    /// enum to select the order of the data of a product page
    /// </summary>
    public enum Order
    {
        Brand,
        Name,
        Price,
        Default
    }

    /// <summary>
    /// class to get search input from an api.
    /// </summary>
    public class ProductPageSearchInput
    {
        public int    pagesize { get; set; }
        public Order orderBy { get; set; }
        public bool   isAsc { get; set; }
        public int    brandId { get; set; }
    }

    /// <summary>
    /// Projection class to hold a product with its categories
    /// </summary>
    public class ProductAndCategories
    {
        public Product Product { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<Category> Categories { get; set;}

    }

    /// <summary>
    /// projection class that hold details of a product
    /// </summary>
    public class ProductDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int GuestUsersRequestsNumber { get; set; }
        public int LoggedUsersRequestsNumber { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        /// <summary>
        /// list of projection of InfoRequests.
        /// </summary>
        public IEnumerable<InfoRequestTemp> Requests { get; set; }
    }

    /// <summary>
    /// projection class of infoRequest  for the ProductDetail class.
    /// Hold the id of the info request, name of the person who made the request, 
    /// number of replies to that request and the date of the last reply.
    /// </summary>
    public class InfoRequestTemp
    {
        public int RequestId { get; set; }
        public string FullName { get; set; }
        public int RepliesCount { get; set; }
        public DateTime LastReply { get; set; }
    }

    /// <summary>
    ///  projection class of Product  for the ProductDetail class
    /// </summary>
    public class ProductSelect
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
    #endregion
}
