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
    public class OrderController : ControllerBase
    {
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int Id, Model.Common.Order value)
        {
            return Ok(new Orders().UpdateStatus(Id,value));
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateTrack([FromRoute] int Id)
        {
            return Ok(new Orders().UpdateTrack(Id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetAll(UserId));
        }
        [HttpGet]
        public async Task<IActionResult> GetTime()
        {
            return Ok(new Orders().GetId());
        }
    }
}
