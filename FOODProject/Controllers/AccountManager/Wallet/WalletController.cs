using FOODProject.Core.AccountManager.Wallet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.AccountManager.Wallet
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSubscriber()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Wallets().GetWallet(UserId));
        }
        [HttpPut]
        public IActionResult AddBalance(Model.AccountManager.Wallet.Wallet value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            return Ok(new Wallets().AddBalance(value,UserId));
        }
    }
}
