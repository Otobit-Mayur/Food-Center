using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop.Categories
{
    public class Categories
    {
     
        FoodCenterDataContext context = new FoodCenterDataContext();


        public Result AddCategory(Model.Shop.Category.Category value)
        {   
            Category c = new Category();
            c.CategoryName = value.CategoryName;
            var check = context.Categories.FirstOrDefault(x => x.CategoryName == value.CategoryName);
            if(check!=null)
            {
                return new Result()
                {
                    Message = string.Format("Category Already Exits"),
                    Status = Result.ResultStatus.warning,
                };
            }
            else
            {
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format("New Category Added Successfully"),
                    Status = Result.ResultStatus.success,
                };
            }

        }
        public Result get()
        {
            return new Result()
            {
                Message=string.Format("Get All Category Successfully"),
                Status=Result.ResultStatus.success,
                Data= (from obj in context.Categories
                       select new { obj.CategoryId, obj.CategoryName }).ToList(),
            };
    
        }

    }
}
