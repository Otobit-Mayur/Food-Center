using FOODProject.Core.Shop.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Shop.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly Categories _categories;

        public CategoryController(Core.Shop.Categories.Categories categories)
        {
            _categories = categories;
        }
        [HttpPost("Category")]
        public IActionResult AddCategory(Model.Shop.Category.Category value)
        {
            var result = _categories.AddCategory(value);
            return Ok(result);

        }
        [HttpGet("Categories")]
        public IActionResult GetAllRole()
        {

            // return Ok(new Accounts().get());
            var result = _categories.Get();
            return Ok(result);
        }
       
    }
}
