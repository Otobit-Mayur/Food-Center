using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.AccountManager.Wallet
{
    public class Wallet
    {
        [Required(ErrorMessage = "Amount is required")]
        public float AddAmount { get; set; }
    }
}
