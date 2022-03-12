using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Common
{
    public class Order
    {
        public Model.Common.IntegerNullString Time { get; set; } = new Model.Common.IntegerNullString();
    }
}
