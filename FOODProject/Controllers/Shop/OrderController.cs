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
    [Route("Common/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPut("{Id}")]
        public 
            
            
            
            IActionResult UpdateStatus([FromRoute] int Id, Model.Shop.Order value)
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
        public IActionResult OrderFilterSort([FromBody] string Filter, int Sorting, int SortingOrder)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().OrderFilterSort(UserId,Filter,Sorting,SortingOrder));
        }
    }
}
