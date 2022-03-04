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
        public string Addresss { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    
    }
}
