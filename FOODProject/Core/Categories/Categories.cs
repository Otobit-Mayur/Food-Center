using FoodCenterContext;
using FOODProject.Model.Category;
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


        public async Task<string>AddCategory(Model.Category.CategoryModel value)
        {
            Category c = new Category();
            c.Category1 = value.Category;
            var check = context.Categories.FirstOrDefault(x => x.Category1 == value.Category);
            if(check!=null)
            {
                return "Category already exist";
            }
            else
            {
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
                return "New Category Added Successfully";
            }

           

        }
        public async Task<IEnumerable> get()
        {
            var qs = (from obj in context.Categories
                      select new { obj.CategoryId, obj.Category1 }).ToList();
            /*var qs = (from obj in context.Categories
                      select obj).ToList();*/
            return qs;
        }

    }
}
