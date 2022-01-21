using System.Collections.Generic;

namespace EFWebSiteTest
{
    public class Category:EntityBase
    {
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
