using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Shop.ShopDetail
{
    public class ShopDetail
    {
        [Required(ErrorMessage = "Shop Name is required")]
        public string ShopName { get; set; }


        [Required(ErrorMessage = "Shop Phone Number is required")]
        public ulong PhoneNumber { get; set; }

        public ulong AlternateNumber { get; set; }

        [Required, Range(1, 10)]
        public int DeliveryRadius { get; set; }

        public string Logo { get; set; }


        public Model.Common.IntegerNullString CategoryId { get; set; } = new Model.Common.IntegerNullString();

        public Model.Common.IntegerNullString UserId { get; set; } = new Model.Common.IntegerNullString();


        [Required(ErrorMessage = "Address is required!")]

        public Model.Shop.ShopDetail.ShopAddress Address { get; set; } = new Model.Shop.ShopDetail.ShopAddress();

    }
    public class UpdateShopDetails
    {
        public string ShopName { get; set; }
        public ulong PhoneNumber { get; set; }
        public string Logo { get; set; }
        public string CoverPhoto { get; set; }
        public string Description { get; set; }

    }
}
