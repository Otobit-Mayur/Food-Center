using FOODProject.Core.AccountManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.AccountManager
{
    [Route("[controller]")]
    [ApiController]
    public class OfficeHomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOfficeDetail()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new OfficesHome().GetOfficeDetail(UserId));
        }
        [HttpGet("GetCategory")]
        public IActionResult GetAllCategory()
        {
            return Ok(new OfficesHome().GetCategory());
        }
    }
}
