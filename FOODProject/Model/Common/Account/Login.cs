using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Common.Account
{
    public class Login
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
