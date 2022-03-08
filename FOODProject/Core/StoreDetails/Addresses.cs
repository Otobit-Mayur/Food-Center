using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.StoreDetails
{
    public class Addresses
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result Addresss(Model.StoreDetail.Address value)
        {
            //dbo.Address
            FoodCenterContext.Address add = new FoodCenterContext.Address();
            add.Address1 = value.Addresss;
            add.Latitude = value.Latitude;
            add.Longitude = value.Longitude;

            var res = context.ShopDetails.SingleOrDefault(x => x.ShopId == value.ShopId.Id);

            add.ShopId = res.ShopId;

            context.Addresses.InsertOnSubmit(add);
            context.SubmitChanges();
            return new Result()
            {
                Message = string.Format($"Address Added"),
                Status = Result.ResultStatus.success,
            };
        }
    }
}
