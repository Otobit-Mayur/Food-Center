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
    public class SubscriberController : ControllerBase
    {
        [HttpGet()]
        public IActionResult GetAllSubscriber()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetAllSubscriber(UserId));
        }
        [HttpGet]
        public IActionResult GetById([FromQuery] int Id)
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
        [HttpPut]
        public IActionResult ApproveRequest([FromQuery]int Id)
        {
            return Ok(new Subscribers().ApproveRequest(Id));
        }
        [HttpPut]
        public IActionResult RejectRequest([FromQuery] int Id)
        {
            return Ok(new Subscribers().RejectRequest(Id));
        }
        [HttpGet("{Id}")]
        public IActionResult GetResentOrder([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetResentOrder(UserId, Id));
        }
        [HttpPut("{Id}")]
        public IActionResult CancelSubscription([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().CancelSubscription(Id, UserId));
        }
    }
}

