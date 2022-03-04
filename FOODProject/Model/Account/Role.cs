using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FOODProject.Model.Account
{
    public class Role
    {
        [Required(ErrorMessage = "RoleName is required")]
        public string role { get; set; }
    }
}
