using System.Collections.Generic;

namespace EFWebSiteTest
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }


        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection< ProductCategory> ProductCategory { get; set; }

        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
