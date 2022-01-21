using System.Collections.Generic;

namespace EFWebSiteTest
{
    public class ProductCategory
    {
        public int IdProduct { get; set; }
        public int IdCategory { get; set; }

        public  Product Product { get; set; }
        public Category Category { get; set; }
    }
}
