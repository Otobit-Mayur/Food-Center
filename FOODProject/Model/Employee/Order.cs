using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Employee
{
    public class Order
    {
        [Required(ErrorMessage = "Product Id is Required")]
        public Model.Common.IntegerNullString Product { get; set; } = new Common.IntegerNullString();

       // public String Description { get; set; }
    }

    public class Ordertime
    {
        [Required (ErrorMessage = "Time Is Required")]
        public Model.Common.IntegerNullString Time { get; set; } = new Model.Common.IntegerNullString();
    }
}
