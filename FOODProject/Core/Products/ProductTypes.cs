using FoodCenterContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Products
{
    public class ProductTypes
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public async Task<string> AddProducttype(Model.Product.ProductType value)
        {
            ProductType pt = new ProductType();
            pt.Type = value.Type;
            var check = context.ProductTypes.SingleOrDefault(x => x.Type==value.Type);
            if (check != null)
            {
                return "Type Already Added";
            }
            else
            {
                context.ProductTypes.InsertOnSubmit(pt);
                context.SubmitChanges();
                return "ProductType Added Successfully";
            }
        }
        public async Task<IEnumerable> get()
        {
            var q = (from obj in context.ProductTypes
                     select new { obj.TypeId, obj.Type }).ToList();

            return q;
        }
    }
}
