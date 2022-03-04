using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FOODProject.Model.Category
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Category Name is required")]
        public String Category { get; set; }
    }
}
