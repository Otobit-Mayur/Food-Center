
using FOODProject.Core.Shop.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Shop.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypes _productTypes;

        public ProductTypeController(Core.Shop.Products.ProductTypes productTypes )
        {
            _productTypes = productTypes;
        }
        //[Authorize]
        [HttpPost("Addtype")]
        public IActionResult AddType(Model.Shop.Product.ProductType value)
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _productTypes.AddProducttype(value, UserId);
            return Ok(result);
        }

        [HttpGet("get")]
        public IActionResult GetType()
        {
            var result = _productTypes.get();
            return Ok(result);
        }
    }
}
