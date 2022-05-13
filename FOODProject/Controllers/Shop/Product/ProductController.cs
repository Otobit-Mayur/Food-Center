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
    [Route("[controller]")]
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
            var RoleID = HttpContext.Items["RoleID"];
            if (RoleID.ToString() == "1")
            {
                var result = _products.AddProuct(value);
                return Ok(result);
            }
            else 
            {
                throw new Exception("Unauthorized");
            }
        }
        [HttpGet("GetFoodType")]
        public IActionResult GetFoodType()
        {
            var result = _products.GetFoodType();
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            int UserId = (int)HttpContext.Items["UserId"];
            var result = _products.GetAllProduct(UserId);
            return Ok(result);
        }
        [HttpGet("GetById")]
        public IActionResult GetById(int Id)
        {
            var result = _products.GetById(Id);
            return Ok(result);
        }
        [HttpPut("UpdateDetail")]
        public IActionResult Update(Model.Shop.Product.Product value,int id)
        {
            var result = _products.Update(value, id);
            return Ok(result);
        }
        [HttpPut("Delete")]
        public IActionResult Delete(int Id)
        {
            var result = _products.Delete(Id);
            return Ok(result);
        }
        [HttpPut("UpdateStatus")]
        public IActionResult UpdateStatus(int id)
        {
            var result = _products.UpdateStatus(id);
            return Ok(result);
        }
        
       
        [HttpGet("Getbytype")]
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
