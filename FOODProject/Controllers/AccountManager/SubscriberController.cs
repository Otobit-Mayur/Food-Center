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
    public class SubscriberController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSubscriber()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetAllSubscriber(UserId));
        }
        [HttpPut("{Id}")]
        public IActionResult UpdateStatus([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().UpdateStatus(Id,UserId));
        }
        [HttpPut("{Id}")]
        public IActionResult CancelSubscription([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().CancelSubscription(Id, UserId));
        }
    }
}

   
       

