using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Core.StoreDetails
{
    public class Addresses
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result Addresss(Model.StoreDetail.Address value)
        {
            //dbo.Address

            using (FoodCenterDataContext context = new FoodCenterDataContext()) {

                FoodCenterContext.Address add = new FoodCenterContext.Address();
                FoodCenterContext.ShopDetail shopDetail = new ShopDetail();
                add.Address1 = value.Addresss;
                add.Latitude = value.Latitude;
                add.Longitude = value.Longitude;
                var res = (from obj in context.ShopDetails
                           where obj.ShopId == value.ShopId.Id
                           select obj).SingleOrDefault();
                add.ShopId = res.ShopId;
               /* shopDetail.IsCompleted = 1;*/
                res.IsCompleted = 1;
                context.Addresses.InsertOnSubmit(add);
                context.SubmitChanges();

               
            }
                
            return new Result()
            {
                Message = string.Format($"Address Added"),
                Status = Result.ResultStatus.success,
            };
        }
    }
}
