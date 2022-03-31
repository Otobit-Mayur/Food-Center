﻿using FOODProject.Core.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Employee
{
    [Route("Employee/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("{Id}")]
        public IActionResult AddOrder([FromRoute] int Id,Model.Employee.Order Value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().AddOrder(Value, UserId, Id));
        }
        [HttpGet]
        public IActionResult GetAllSubscriber()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Orders().GetAllSubscriber(UserId));
        }
        [HttpGet("{Id}")]
        public IActionResult GetAllProduct([FromRoute] int Id)
        {
            return Ok(new Orders().GetAllProduct(Id));
        }
    }
}

