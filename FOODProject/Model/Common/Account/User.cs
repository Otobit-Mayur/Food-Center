    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Common.Account
{
    public class User
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password required")]
        [Compare("Password")]
        public string Confirm_Password { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        public Model.Common.IntegerNullString RoleId { get; set; } = new Model.Common.IntegerNullString();

    }
    public class Changepassword
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}
