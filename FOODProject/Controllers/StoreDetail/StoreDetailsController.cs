using FOODProject.Core.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreDetailsController : ControllerBase
    {
        private readonly StoreDetails _store_Details;

        public StoreDetailsController(StoreDetails store_Details)
        {
            _store_Details = store_Details;
        }
       [HttpPost("StoreDetails")]
        public async Task<IActionResult> Add(Model.StoreDetail.StoreDetail values)
        {
            //return Ok(new Accounts().AddRole(role));
            var result = _store_Details.AddStoreDetails(values);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult>Update(Model.StoreDetail.Update value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _store_Details.UpdateProfile(value,UserId);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetAllUser()
        {
            var result = _store_Details.getallShop();
            return Ok(result);
        }
    }
}
