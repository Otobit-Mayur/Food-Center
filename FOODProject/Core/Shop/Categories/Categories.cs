using FoodCenterContext;
using FOODProject.Model.Category;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Categories
{
    public class Categories
    {
        private readonly CategoryModel _category;
        FoodCenterDataContext context = new FoodCenterDataContext();


        public Result AddCategory(Model.Category.CategoryModel value)
        {   
            Category c = new Category();
            c.CategoryName = value.Category;
            var check = context.Categories.FirstOrDefault(x => x.CategoryName == value.Category);
            if(check!=null)
            {
                return new Result()
                {
                    Message = string.Format("Category Already Exits"),
                    Status = Result.ResultStatus.success,
                };
            }
            else
            {
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format("New Category Added Successfully"),
                    Status = Result.ResultStatus.warning,
                };
            }

        }
        public async Task<IEnumerable> get()
        {
            var qs = (from obj in context.Categories
                      select new { obj.CategoryId, obj.CategoryName }).ToList();
            return qs;
        }

    }
}
