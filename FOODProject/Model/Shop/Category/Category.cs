using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Shop.Category
{
    public class Category
    {
        [Required(ErrorMessage = "Category Name is required")]
        public String CategoryName { get; set; }
    }
}
