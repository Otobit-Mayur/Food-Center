using FOODProject.Core.AccountManager.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.AccountManager.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddEmployeeController : ControllerBase
    {
        private readonly AddEmployees _addEmployees;

        public AddEmployeeController(Core.AccountManager.Employees.AddEmployees addEmployees)
        {
            _addEmployees = addEmployees;
        }
        [HttpPost]
        public IActionResult AddEmployee(Model.AccountManager.Employee.AddEmp value)
        {
            var result = _addEmployees.AddEmployee(value);
            return Ok(result);

        }
        [HttpPut("{Id}")]
        public IActionResult AddempPassword(Model.AccountManager.Employee.AddEmployeePassword value,int Id)
        {
            var result = _addEmployees.AddEmpPassword(value,Id);
            return Ok(result);
        }
    }
}
