using FOODProject.Core.Shop.StoreDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Shop.StoreDetail
{
    [Route("[controller]")]
    [ApiController]
    public class ShopDetailsController : ControllerBase
    {
        private readonly StoreDetails _storeDetails;

        public ShopDetailsController(Core.Shop.StoreDetails.StoreDetails storeDetails)
        {
            _storeDetails = storeDetails;
        }
        [HttpPost]
        public IActionResult Add(Model.Shop.ShopDetail.ShopDetail value)
        {
            var result = _storeDetails.AddStoreDetails(value);
            return Ok(result);
        }
        [HttpGet]

        public IActionResult GetAllShop()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _storeDetails.GetCurrentShop(UserId);
            return Ok(result);
        }
        [HttpPut]
        public IActionResult Update(Model.Shop.ShopDetail.UpdateShopDetails value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _storeDetails.UpdateProfile(value, UserId);
            return Ok(result);
        }
    }
}
   