using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProductAndCategoryModel
    {
        public Product Product { get; set; }
        public List<int> Categories { get; set; }
    }

    public class ProductAndCategoryModel2
    {
        //public Product Product { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }

        public List<int> Categories { get; set; }
    }
}
