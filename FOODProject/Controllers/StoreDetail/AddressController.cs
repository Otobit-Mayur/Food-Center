using FOODProject.Core.StoreDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.StoreDetail
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly Address _address;

        public AddressController(Core.StoreDetails.Address address)
        {
            _address = address;
        }

        [HttpPost("Add")]
        public async Task<IActionResult>AddAddress(Model.StoreDetail.Address value)
        {
            var result = _address.Addresss(value);
            return Ok(result);
        }
    }
}
