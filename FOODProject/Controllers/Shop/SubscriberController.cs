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
    public class SubscriberController : ControllerBase
    {
        [HttpGet("{Id}")]
        public IActionResult GetAllSubscriber()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetAllSubscriber(UserId));
        }
        [HttpGet("{Id}")]
        public IActionResult GetById([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetById(UserId,Id));
        }
        [HttpGet]
        public IActionResult GetAllRequest()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetAllRequest(UserId));
        }
        [HttpGet("{Id}")]
        public IActionResult GetResentOrder([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetResentOrder(UserId, Id));
        }
    }
}

