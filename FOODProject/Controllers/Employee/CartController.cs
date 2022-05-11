using FOODProject.Core.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Employee
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCartOrder()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Carts().GetCartOrder(UserId));
        }
        [HttpDelete("{Id}")]
        public IActionResult DeleteOrder([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Carts().DeleteOrder(Id,UserId));
        }
        [HttpPut("{Id}")]
        public IActionResult OrderRequest([FromRoute] int Id)
        {
            return Ok(new Carts().OrderRequest(Id));
        }
        [HttpGet]
        public IActionResult GetCartHistory()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Carts().GetCartHistory(UserId));
        }
    }
}
