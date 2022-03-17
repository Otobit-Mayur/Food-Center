﻿using FOODProject.Core.Accounts;
using FOODProject.Model.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FOODProject.Core.Accounts.Accounts;

namespace FOODProject.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Accounts _accounts;
        public AccountController(Accounts accounts)
        {
            _accounts = accounts;
        }
        [HttpPost("role")]
        public async Task<IActionResult> Add(Model.Account.Role role)
        {
            //return Ok(new Accounts().AddRole(role));
            var result = _accounts.AddRole(role);
            return Ok(result);

        }
        [HttpGet("role")]
        public IActionResult GetAllRole()
        {

            // return Ok(new Accounts().get());
            var result = _accounts.getallRole();
            return Ok(result);
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> Store_SignUp(Model.Account.User values)
        {
            //return Ok(new Accounts().SignUp(store_SignUpModel));
            var result = _accounts.SignUp(values);
            return Ok(result);
        }
        [HttpGet("User")]
        public IActionResult GetAllUser()
        {

            // return Ok(new Accounts().get());
            var result = _accounts.getallUser();
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(Model.Account.Login values)
        {
            var result = _accounts.Login(values);
            return Ok(result);
        }
        [HttpPost("Distance")]
        public IActionResult Getdistance(Location point1)
        {

            // return Ok(new Accounts().get());
            var result = _accounts.CalculateDistance(point1);
            return Ok(result);
        }

    }
}