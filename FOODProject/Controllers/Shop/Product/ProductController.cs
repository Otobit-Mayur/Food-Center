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
   
    public class ProductController : ControllerBase
    {
        private readonly Products _products;

        public ProductController(Core.Shop.Products.Products products)
        {
            _products = products;
        }

        [HttpPost("Add")]
        public IActionResult AddProduct(Model.Shop.Product.Product value)
        {
            var result = _products.AddProuct(value);
            return Ok(result);
        }
       //[Authorize]
        [HttpGet("get")]
        public IActionResult Get()
        {
            var result = _products.GetAllProduct();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Model.Shop.Product.Product value,int id)
        {
            var result = _products.Update(value, id);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Model.Shop.Product.Product value, int id)
        {
            var result = _products.Delete(value, id);
            return Ok(result);
        }
        [HttpPut]
        public IActionResult UpdateStatus(int id)
        {
            var result = _products.UpdateStatus(id);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult getsids()
        {
            string EmailId = (string)HttpContext.Items["EmailId"];
            //return Ok(new Orders().getsid(EmailId));
            var result = _products.getsid(EmailId);
            return Ok(result);
        }
        [HttpGet("getbytype")]
        public IActionResult GetByType(int TypeId)
        {
            var result = _products.GetByType(TypeId);
            return Ok(result);
        }
        [HttpGet("SortPrice")]
        public IActionResult SortPrice()
        {
            var result = _products.SortByPrice();
            return Ok(result);
        }
        [HttpGet("SortPriceDescending")]
        public IActionResult SortPriceDescending()
        {
            var result = _products.SortByPriceDes();
            return Ok(result);
        }
    }
}
