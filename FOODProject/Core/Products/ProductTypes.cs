using FoodCenterContext;
using FOODProject.Model.Common;
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

        public async Task<string> AddProducttype(Model.Product.ProductType value,int UserId)
        {
            ProductType pt = new ProductType();

            var qes = (from obj in context.ShopDetails
                       where obj.UserId == UserId
                       select obj.ShopId).SingleOrDefault();
            pt.Type = char.ToUpper(value.Type[0]) + value.Type.Substring(1).ToLower();
            pt.ShopId = qes;
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
        /*$distance = DB::raw("( 3959 * acos( cos( radians($lat) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians($lng) ) + sin( radians($lat) ) * sin( radians( latitude ) ) ) ) AS distance");*/
        public async Task<IEnumerable> get()
        {
            var q = (from obj in context.ProductTypes
                     select new { obj.TypeId, obj.Type }).ToList();

            return q;
        }
       

    }
}
