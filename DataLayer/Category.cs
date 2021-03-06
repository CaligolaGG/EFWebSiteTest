using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    /// <summary>
    /// represents a category for the products.
    /// Products can have more than one category.
    /// </summary>
    public class Category:EntityBase
    {
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
