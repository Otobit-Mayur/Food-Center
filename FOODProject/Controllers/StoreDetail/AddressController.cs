//using FOODProject.Core.StoreDetails;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FOODProject.Controllers.StoreDetail
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AddressController : ControllerBase
//    {
//        private readonly Addresses _address;

//        public AddressController(Core.StoreDetails.Addresses address)
//        {
//            _address = address;
//        }

//        [HttpPost("Add")]
//        public async Task<IActionResult>AddAddress(Model.StoreDetail.Address value)
//        {
//            var result = _address.Addresss(value);
//            return Ok(result);
//        }
//        [HttpGet]
//        public IActionResult GetAllUser()
//        {
//            var result = _address.getAddress();
//            return Ok(result);
//        }
//    }
//}
