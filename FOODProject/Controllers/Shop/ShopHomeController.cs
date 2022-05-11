using FOODProject.Core.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Shop
{
    [Route("[controller]")]
    [ApiController]
    public class ShopHomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetShopDetail()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new ShopHomes().GetShopDetail(UserId));
        }
        [HttpPut("UpadateStatus")]
        public IActionResult UpdateStatus()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new ShopHomes().UpdateStatus(UserId));
        }
        [HttpGet("TodaysOrder")]
        public IActionResult TodaysOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new ShopHomes().TodaysOrder(UserId));
        }
        [HttpGet("GetAllProductType")]
        public IActionResult GetType()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new ShopHomes().GetProductType(UserId));
        }
        [HttpGet("GetTopOrder")]
        public IActionResult GetTopOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new ShopHomes().GetTopOrder(UserId));
        }
    }
}
