using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// class for the n:n relationship between product and category
    /// </summary>
    public class ProductCategory
    {
        public int IdProduct { get; set; }
        public int IdCategory { get; set; }

        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
