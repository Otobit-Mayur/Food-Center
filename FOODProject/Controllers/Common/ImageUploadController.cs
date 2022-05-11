using FOODProject.Core.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Common
{
    [Route("[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImageUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpPost("ImageUpload")]
        public IActionResult UploadImage([FromForm] Model.Common.ImageUpload value)
        {
            return Ok(new UploadImage().UploadNewImage(value, _environment));
        }
    }
}
