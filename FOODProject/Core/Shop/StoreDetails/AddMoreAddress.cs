using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop.StoreDetails
{
    public class AddMoreAddress
    {
        public Result AddmoreAddress(Model.Shop.ShopDetail.ShopAddress value,int UserId)
        {
            using(FoodCenterDataContext context =new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj.ShopId).SingleOrDefault();
                if(qes == null)
                {
                    throw new ArgumentException("Shop ID Not Found");
                }
                else
                {
                    FoodCenterContext.ShopAddress add = new FoodCenterContext.ShopAddress()
                    {
                        AddressLine = value.AddresssLine,
                        Latitude = value.Latitude,
                        Longitude = value.Longitude,
                        ShopId = qes,
                    };
                    context.ShopAddresses.InsertOnSubmit(add);
                    context.SubmitChanges();
                    return new Result()
                    {

                        Message = string.Format($"Address Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = add.ShopId,
                    };
                }
            }
        }
        public Result GetAllAddress(int UserId)
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj.ShopId).SingleOrDefault();
                return new Result()
                {
                    Message = string.Format("Get All Address successfully"),
                    Status = Result.ResultStatus.success,
                    Data = (from add in context.ShopAddresses
                            where add.ShopId==qes
                           select new
                           {
                               add.AddressLine,
                               add.Latitude,
                               add.Longitude,
                               add.ShopId,
                           }).ToList(),
                };
            }

            
        }
        public Result UpdateAddress(Model.Shop.ShopDetail.ShopAddress value,int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                ShopAddress address = context.ShopAddresses.SingleOrDefault(x => x.AddressId == Id);
                if (address != null)
                {
                    address.AddressLine = value.AddresssLine;
                    address.Latitude = value.Latitude;
                    address.Longitude = value.Longitude;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Address Updated"),
                        Status = Result.ResultStatus.success,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"Address Id Not Found"),
                    Status = Result.ResultStatus.success,
                };
            }
        }
        public Result DeleteAddress(int id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                ShopAddress address = context.ShopAddresses.SingleOrDefault(x => x.AddressId == id);
                if (address != null)
                {
                    context.ShopAddresses.DeleteOnSubmit(address);
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Address Deleted"),
                        Status = Result.ResultStatus.success,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"Address Id Not Found"),
                    Status = Result.ResultStatus.success,
                };
            }
        }
    }
}
