
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
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Employees().GetAllEmployee(UserId));
        }
        [HttpPut("{Id}")]
        public IActionResult DeleteEmployee([FromRoute] int Id)
        {
            return Ok(new Employees().DeleteEmployee(Id));
        }
        [HttpPut("{Id}")]
        public IActionResult EmployeeStatus([FromRoute] int Id)
        {
            return Ok(new Employees().EmployeeStatus(Id));
        }
    }
}
   
   
       
 



