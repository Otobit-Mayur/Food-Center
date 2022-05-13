using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Shop.Product
{
    public class Product
    {
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public int Price { get; set; }
        public string Description { get; set; }
        // public int Image { get; set; }

        public Model.Common.IntegerNullString FoodType { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString Image { get; set; } = new Model.Common.IntegerNullString();

        public Model.Common.IntegerNullString Type { get; set; } = new Model.Common.IntegerNullString();
       // public Model.Common.IntegerNullString FoodType { get; set; } = new Model.Common.IntegerNullString();
    }
}
