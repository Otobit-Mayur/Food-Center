using FOODProject.Core.AccountManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.AccountManager
{
    [Route("AccountManager/[controller]/[action]")]
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
        [HttpPut("{Id}")]
        public IActionResult RejectOrder([FromRoute] int Id)
        {
            return Ok(new Orders().RejectOrder(Id));
        }
    }
}




