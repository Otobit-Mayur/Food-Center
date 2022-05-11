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

        public string PhoneNumber { get; set; }
        public string AlternateNumber { get; set; }
        //public string Image { get; set; }
        public Model.Common.IntegerNullString Image { get; set; } = new Model.Common.IntegerNullString();

        public Model.Common.IntegerNullString User { get; set; } = new Model.Common.IntegerNullString();
        [Required(ErrorMessage = "Address is required!")]

        public Model.AccountManager.OfficeDetail.OfficeAddress Address { get; set; } = new Model.AccountManager.OfficeDetail.OfficeAddress();

    }
   public class UpdateProfile
    {
     [Required(ErrorMessage = "Manager Name is required!")]
    public string ManagerName { get; set; }

    [Required(ErrorMessage = "Phone Number is required!")]
    public string PhoneNumber { get; set; }
    public Model.Common.IntegerNullString Image { get; set; } = new Model.Common.IntegerNullString();
    public Model.Common.IntegerNullString Cover { get; set; } = new Model.Common.IntegerNullString();
    public String Description { get; set; }
    public Model.AccountManager.OfficeDetail.OfficeAddress Address { get; set; } = new Model.AccountManager.OfficeDetail.OfficeAddress();
    }
}
