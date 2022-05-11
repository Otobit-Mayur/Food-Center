using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers
{
    
        [Route("test")]
        [ApiController]
        public class TestController : ControllerBase
        {

            [HttpGet]
            public IActionResult GetAllNewRequest()
            {
                return Ok("Working!!");
            }
        
    }
}
