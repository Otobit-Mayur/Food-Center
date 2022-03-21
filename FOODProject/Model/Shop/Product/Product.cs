using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Shop.Product
{
    public class Product
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public Model.Common.IntegerNullString TypeId { get; set; } = new Model.Common.IntegerNullString();
    }
}
