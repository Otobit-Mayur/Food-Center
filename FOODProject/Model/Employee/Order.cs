using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Employee
{
    public class Order
    {
        public Model.Common.IntegerNullString Product { get; set; } = new Common.IntegerNullString();

        public String Description { get; set; }
    }
}

