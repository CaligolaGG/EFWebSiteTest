using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer
{
    /// <summary>
    /// Account of type brand.
    /// Represents a brand that produces the products in the db
    /// </summary>
    public class Brand : EntityBase
    {
        public int AccountId { get; set; }
        
        [MaxLength(50)]
        public string BrandName { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public Account Account { get; set; }

        [ForeignKey("UserId")]
        public ICollection<Product> Products { get; set; }

    }
}
