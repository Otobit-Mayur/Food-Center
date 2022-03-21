using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Shop.ShopDetail
{
    public class ShopAddress
    {
        [Required(ErrorMessage = "Address is required")]
        public string AddresssLine { get; set; }

        [Required(ErrorMessage = "Latitude is required")]

        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        public decimal Longitude { get; set; }
    }
}
