using FOODProject.Core.Common.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Common.Account
{
    [Route("[controller]")]
    [ApiController]
    public class PasswordChangeController : ControllerBase
    {
        private readonly UpdatePassword _updatePassword;

        public PasswordChangeController(Core.Common.Accounts.UpdatePassword updatePassword)
        {
            _updatePassword = updatePassword;
        }

        [HttpPut]
        public IActionResult Updatepassword(Model.Common.Account.Changepassword value,[FromQuery]int id)
        {
           // int UserId = (int)HttpContext.Items["UserId"];
            var result = _updatePassword.Changepassword(value,id);
            return Ok(result);
        }
    }
}
