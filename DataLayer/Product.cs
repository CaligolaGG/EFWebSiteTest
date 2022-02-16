using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// Represent a product.
    /// A product belongs to one brand and can have multiple categories.
    /// </summary>
    public class Product : EntityBase
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(20)]
        public string ShortDescription { get; set; }

        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }


        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductCategory> ProductCategory { get; set; }

        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
