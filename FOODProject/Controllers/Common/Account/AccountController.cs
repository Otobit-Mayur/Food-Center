using FOODProject.Core.Common.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FOODProject.Core.Common.Accounts.Accounts;

namespace FOODProject.Controllers.Common.Account
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
       /* [HttpPost("role")]
        public IActionResult Add(Model.Common.Account.Role role)
        {
            //return Ok(new Accounts().AddRole(role));
            var result = _accounts.AddRole(role);
            return Ok(result);

        }*/
        [HttpGet("role")]
        public IActionResult GetAllRole()
        {

            // return Ok(new Accounts().get());
            var result = _accounts.GetallRole();
            return Ok(result);
        }
        [HttpPost("Signup")]
        public IActionResult Store_SignUp(Model.Common.Account.User values)
        {
            //return Ok(new Accounts().SignUp(store_SignUpModel));
            var result = _accounts.SignUp(values);
            return Ok(result);
        }
        [HttpPost("CheckMail")]
        public IActionResult MailCheck(Model.Common.Account.CheckEmail value)
        {
            var result = _accounts.CheckEmail(value);
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
        public IActionResult login(Model.Common.Account.Login values)
        {
            var result = _accounts.Login(values);
            return Ok(result);
        }
        /*[HttpPost("Distance")]
        public IActionResult Getdistance(Location point1)
        {

            // return Ok(new Accounts().get());
            var result = _accounts.CalculateDistance(point1);
            return Ok(result);
        }*/
        [HttpGet("GetCurrentUser")]
        public IActionResult Getsid()
        {
            string EmailId = (string)HttpContext.Items["EmailId"];
            //return Ok(new Orders().getsid(EmailId));
            var result = _accounts.getsid(EmailId);
            return Ok(result);
        }
       
    }
}
