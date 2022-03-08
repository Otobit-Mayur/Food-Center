﻿using FOODProject.Core.Products;
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
    public class ProductController : ControllerBase
    {
        private readonly Products _products;

        public ProductController(Products products)
        {
            _products = products;
        }

        [HttpPost("Add")]
        public async Task<IActionResult>AddProduct(Model.Product.Product value)
        {
            var result = _products.AddProuct(value);
            return Ok(result);
        }
        [HttpGet("get")]
        public async Task<IActionResult>Get()
        {
            var result = _products.get();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Model.Product.Product value,int id)
        {
            var result = _products.Update(value, id);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Model.Product.Product value, int id)
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
    }
}