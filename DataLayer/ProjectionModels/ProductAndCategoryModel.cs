using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProductAndCategoryModel
    {
        public Product Product { get; set; }
        public List<int> categories { get; set; }
    }
}
