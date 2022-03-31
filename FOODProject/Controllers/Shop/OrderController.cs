using FOODProject.Core.Common;
using FOODProject.Core.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Shop
{
    [Route("Shop/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPut("{Id}")]
        public IActionResult UpdateStatus([FromRoute] int Id, Model.Employee.Ordertime value)
        {
            return Ok(new Orders().UpdateStatus(Id,value));
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateTrack([FromRoute] int Id)
        {
            return Ok(new Orders().UpdateTrack(Id));
        }
        [HttpGet]
        public IActionResult GetAllOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetAllOrder(UserId));
        }
        [HttpGet]
        public IActionResult GetOrderHistory()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetOrderHistory(UserId));
        }
        [HttpGet]
        public IActionResult GetTime()
        {
            return Ok(new Orders().GetTime());
        }
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(new Orders().GetStatus());
        }
        [HttpGet]
        public IActionResult GetTrack()
        {
            return Ok(new Orders().GetTrack());
        }
        [HttpGet]
        public IActionResult OrderFilterSort(Model.Common.FilterSort Value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().OrderFilterSort(UserId,Value));
        }
        [HttpGet]
        public IActionResult TodaysOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().TodaysOrder(UserId));
        }
        [HttpGet]
        public IActionResult TodaysDeliveredOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().TodaysDeliveredOrder(UserId));
        }
        [HttpGet]
        public IActionResult GetTopOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetTopOrder(UserId));
        }
    }
}
