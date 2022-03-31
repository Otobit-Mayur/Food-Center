using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Common
{
    public class FilterSort
    {    
        public string Filter { get; set; }
        public int Sorting { get; set; }
        public int SortingOrder { get; set; }

    }
    public enum Sort { 
    Quantity = 14,
    Distance = 15,

    }
}

