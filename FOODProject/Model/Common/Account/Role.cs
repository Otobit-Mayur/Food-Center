using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Common.Account
{
    public class Role
    {
        [Required(ErrorMessage = "RoleName is required")]
        public string role { get; set; }
    }
}
