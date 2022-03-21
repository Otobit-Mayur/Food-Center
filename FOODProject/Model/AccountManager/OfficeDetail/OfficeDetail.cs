using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.AccountManager.OfficeDetail
{
    public class OfficeDetail
    {
        [Required(ErrorMessage = "Office Name is required!")]
        public string OfficeName { get; set; }

        [Required(ErrorMessage = "Manager Name is required!")]
        public string Managername { get; set; }

        [Required(ErrorMessage = "Phone Number is required!")]

        public ulong PhoneNumber { get; set; }
        public ulong AlternateNumber { get; set; }
        public string Image { get; set; }

        public Model.Common.IntegerNullString UserId { get; set; } = new Model.Common.IntegerNullString();
        [Required(ErrorMessage = "Address is required!")]

        public Model.AccountManager.OfficeDetail.OfficeAddress Address { get; set; } = new Model.AccountManager.OfficeDetail.OfficeAddress();

    }
   public class UpdateProfile
    {
     [Required(ErrorMessage = "Manager Name is required!")]
    public string ManagerName { get; set; }

    [Required(ErrorMessage = "Phone Number is required!")]
    public ulong PhoneNumber { get; set; }
    public string Image { get; set; }
    public string CoverPhoto { get; set; }
    public String Description { get; set; }
}
}
