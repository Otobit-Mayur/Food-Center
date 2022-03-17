using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FOODProject.Model.Account
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
