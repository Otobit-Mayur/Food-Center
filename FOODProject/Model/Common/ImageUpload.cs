using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.Common
{
    public class ImageUpload
    {
        public IFormFile Files { get; set; }
    }
}
