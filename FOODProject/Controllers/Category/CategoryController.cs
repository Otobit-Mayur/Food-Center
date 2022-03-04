using FOODProject.Core.Accounts;
using FOODProject.Core.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly Categories _categories;

        public CategoryController(Core.Categories.Categories categories)
        {
            _categories = categories;
        }
        [HttpPost("Category")]
        public async Task<IActionResult> AddCategory(Model.Category.CategoryModel value)
        {
            var result = _categories.AddCategory(value);
            return Ok(result);

        }
        [HttpGet("Categories")]
        public IActionResult GetAllRole()
        {

            // return Ok(new Accounts().get());
            var result = _categories.get();
            return Ok(result);
        }
       
    }
}
