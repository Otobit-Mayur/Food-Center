using FOODProject.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Common
{
    [Route("Common/[controller]/[action]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSubscriber()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetAllSubscriber(UserId));
        }
        [HttpGet("{1d}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetById(UserId,Id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRequest()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Subscribers().GetAllRequest(UserId));
        }
    }
}

