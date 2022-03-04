using FOODProject.Core.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpadateUserController : ControllerBase
    {
        private readonly UpdatePassword _updatePassword;

        public UpadateUserController(Core.Accounts.UpdatePassword updatePassword)
        {
            _updatePassword = updatePassword;
        }

        [HttpPut]
        public async Task<IActionResult>Updatepassword(Model.Account.Changepassword value,int id)
        {
            var result = _updatePassword.Changepassword(value, id);
            return Ok(result);
        }
    }
}
