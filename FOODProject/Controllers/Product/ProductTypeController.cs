using FOODProject.Core.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypes _productTypes;

        public ProductTypeController(ProductTypes productTypes )
        {
            _productTypes = productTypes;
        }
        [HttpPost("Addtype")]
        public async Task<IActionResult> AddType(Model.Product.ProductType value)
        {
            var result = _productTypes.AddProducttype(value);
            return Ok(result);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetType()
        {
            var result = _productTypes.get();
            return Ok(result);
        }
    }
}
