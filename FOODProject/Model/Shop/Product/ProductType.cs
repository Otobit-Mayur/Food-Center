using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Shop.Product
{
    public class ProductType
    {
        [Required(ErrorMessage = "Product Type is required")]
        public string Type { get; set; }
    }
}
