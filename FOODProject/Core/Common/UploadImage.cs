using FoodCenterContext;
using FOODProject.Model.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Common
{
    public class UploadImage
    {
        FoodCenterDataContext db = new FoodCenterDataContext();
        public Result UploadNewImage(Model.Common.ImageUpload value, IWebHostEnvironment environment)
        {
            var ext = Path.GetExtension(value.Files.FileName).ToLowerInvariant();

            if (value.Files.Length <= 0)
            {
                throw new ArgumentException("File is Empty!");
            }
            var IGUID = Guid.NewGuid();
            var path = "";
            var basePath = environment.WebRootPath + "\\Upload\\";
            var FileName = IGUID + "_" + value.Files.FileName;

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            using (FileStream fileStream = System.IO.File.Create(basePath + FileName))
            {
                value.Files.CopyTo(fileStream);
                fileStream.Flush();
                path = $"Upload/" + FileName;
            }

            Image image = new Image()
            {
                FileName = FileName,
                IGUID = IGUID,
                Path = path,
                SizeInByte = value.Files.Length,
                CreatedDate = DateTime.Now.ToLocalTime(),
                ModifiedDate = DateTime.Now.ToLocalTime(),
                IsDeleted = true,
            };
            db.Images.InsertOnSubmit(image);
            db.SubmitChanges();
            return new Result()
            {
                Message = string.Format($"File created successfully!"),
                Status = Result.ResultStatus.success,
                Data = new
                {
                    Id = image.ImageId,
                    Text = path
                }
            };
        }
    }
}
