using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Employee
{
    public class AddEmp
    {
        [Required(ErrorMessage = "EmialId is required")]
        [EmailAddress]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string OfficeLocation { get; set; }
        
        //public Model.Shop.ShopDetail.ShopAddress Address { get; set; } = new Model.Shop.ShopDetail.ShopAddress();
        //public Model.AccountManager.Employee.EmployeeDetail Detail { get; set; } = new Model.AccountManager.Employee.EmployeeDetail();
    }
    public class AddEmployeePassword
    {
        public string Password { get; set; }
    }
}
