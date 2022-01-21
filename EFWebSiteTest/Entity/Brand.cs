using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFWebSiteTest
{
    public class Brand : EntityBase
    {
        public int AccountId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }

        public Account Account { get; set; }

        [ForeignKey("UserId")]
        public ICollection<Product> Products { get; set; }

    }
}
