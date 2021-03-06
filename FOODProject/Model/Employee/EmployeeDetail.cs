using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Employee
{
    public class EmployeeDetail
    {
        [Required(ErrorMessage = "Employee Name is required")]
        public string EmployeeName { get; set; }


        [Required(ErrorMessage = "Employee Phone Number is required")]
        public ulong PhoneNumber { get; set; }
        //public string Photo { get; set; }
        public Model.Common.IntegerNullString Photo { get; set; } = new Model.Common.IntegerNullString();

        // public Model.Common.IntegerNullString UserId { get; set; } = new Model.Common.IntegerNullString();
    }
    public class UpdateEmployeeDetail
    {
        [Required(ErrorMessage = "Employee Name is required")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Employee Phone Number is required")]
        public ulong PhoneNumber { get; set; }
         public Model.Common.IntegerNullString Photo { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString CoverPhoto { get; set; } = new Model.Common.IntegerNullString();
    }
}
