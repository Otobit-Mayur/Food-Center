using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop.Products
{
    public class ProductTypes
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result AddProducttype(Model.Shop.Product.ProductType value,int UserId)
        {
            ProductType pt = new ProductType();

            var qes = (from obj in context.ShopDetails
                       where obj.UserId == UserId
                       select obj.ShopId).SingleOrDefault();
            pt.Type = char.ToUpper(value.Type[0]) + value.Type.Substring(1).ToLower();
            pt.ShopId = qes;
            var check = context.ProductTypes.SingleOrDefault(x => x.Type==value.Type && x.ShopId==qes);
            if (check != null)
            {
                return new Result()
                {
                    Message = string.Format("Type Already Added"),
                    Status = Result.ResultStatus.warning,
                };
            }
            else
            {
                context.ProductTypes.InsertOnSubmit(pt);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format("ProductType Added Successfully"),
                    Status = Result.ResultStatus.warning,
                };
            }
        }
        /*$distance = DB::raw("( 3959 * acos( cos( radians($lat) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians($lng) ) + sin( radians($lat) ) * sin( radians( latitude ) ) ) ) AS distance");*/
        public Result get()
        {
            return new Result()
            {
                Message = string.Format("Get All Product Type Sccessfully"),
                Status = Result.ResultStatus.warning,
                Data = (from obj in context.ProductTypes
                        select new { obj.TypeId, obj.Type }).ToList(),
            };
        }
       

    }
}
