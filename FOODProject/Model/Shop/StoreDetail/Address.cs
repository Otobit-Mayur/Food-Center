using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FOODProject.Model.StoreDetail
{
    public class Address
    {
        [Required(ErrorMessage = "Address is required")]
        public string AddresssLine { get; set; }

        [Required(ErrorMessage = "Latitude is required")]

        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        public decimal Longitude { get; set; }

        //public Model.Common.IntegerNullString ShopId { get; set; } = new Model.Common.IntegerNullString();
    }
}
