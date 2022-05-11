using FOODProject.Core.AccountManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.AccountManager
{
    [Route("Office/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllOrderRequest()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetAllOrderRequest(UserId));
        }
        [HttpPut("{Id}")]
        public IActionResult DeleteOrder([FromRoute] int Id)
        {
            return Ok(new Orders().DeleteOrder(Id));
        }
        [HttpPut("{Id}")]
        public IActionResult ApproveOrder([FromRoute] int Id)
        {
            return Ok(new Orders().ApproveOrder(Id));
        }
        [HttpPut]
        public IActionResult Checkout()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().Checkout(UserId));
        }
        [HttpPut("{Id}")]
        public IActionResult RejectOrder([FromRoute] int Id)
        {
            return Ok(new Orders().RejectOrder(Id));
        }
        [HttpGet]
        public IActionResult GetAllApproved()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetAllApproved(UserId));
        }
        /*[HttpPut]
        public IActionResult Ckeckout()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().Checkout(UserId));
        }*/
    }
}




