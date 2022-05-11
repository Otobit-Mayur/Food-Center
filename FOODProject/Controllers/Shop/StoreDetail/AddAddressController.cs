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
    public class AddAddressController : ControllerBase
    {
        private readonly AddMoreAddress _addMoreAddress;

        public AddAddressController(Core.Shop.StoreDetails.AddMoreAddress addMoreAddress)
        {
            _addMoreAddress = addMoreAddress;
        }
        [HttpPost]
        public IActionResult AddAddress(Model.Shop.ShopDetail.ShopAddress value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _addMoreAddress.AddmoreAddress(value, UserId);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetAllAddress()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _addMoreAddress.GetAllAddress(UserId);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Model.Shop.ShopDetail.ShopAddress value, int id)
        {
            var result = _addMoreAddress.UpdateAddress(value, id);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _addMoreAddress.DeleteAddress(id);
            return Ok(result);
        }
    }
}
