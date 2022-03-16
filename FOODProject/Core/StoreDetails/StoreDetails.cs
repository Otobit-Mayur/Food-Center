using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Core.Accounts
{
    public class StoreDetails
    {
        public Result AddStoreDetails(Model.StoreDetail.StoreDetail value)
        {

          /*  using (TransactionScope scope = new TransactionScope())
            {*/
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                    ShopDetail sd = new ShopDetail();

                    sd.IsCompleted = 1;
                    sd.ShopName = value.ShopName;
                    sd.PhoneNumber = value.PhoneNumber;
                    sd.AlternateNumber = value.AlternateNumber;
                    sd.DeliveryRadius = value.DeliveryRadius;
                    sd.Logo = value.Logo;
                    sd.Status = 1;

                    var ci = context.Categories.SingleOrDefault(c => c.Category1 == value.CategoryId.String);

                    var ui = context.Users.SingleOrDefault(c => c.EmailId == value.UserId.String);
                    var check = context.ShopDetails.SingleOrDefault(c=>c.UserId==ui.UserId);
                        if (check != null) 
                        {
                            throw new ArgumentException("Entered Email Already Registered for another store");
                        }


                    sd.CategoryId = ci.CategoryId;
                    sd.UserId = ui.UserId;

                    context.ShopDetails.InsertOnSubmit(sd);
                    context.SubmitChanges();


                    FoodCenterContext.Address add = new FoodCenterContext.Address() 
                    { 
                        Address1 = value.Address.AddresssLine,
                        Latitude = value.Address.Latitude,
                        Longitude = value.Address.Longitude,
                        ShopId = sd.ShopId,
                    };
                    context.Addresses.InsertOnSubmit(add);
                    context.SubmitChanges();
                   // scope.Complete();
                    return new Result()
                    {

                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = sd.ShopId,
                    };
               }
            
        }
        public Result getallShop()
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                return new Result()
                {
                    Message = String.Format($"Get All Shop Details"),
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.ShopDetails
                            select new
                            {
                                obj.ShopId,
                                obj.ShopName,
                                obj.PhoneNumber,
                                obj.DeliveryRadius,
                                obj.UserId,
                                obj.CategoryId,
                                obj.Status,
                                obj.IsCompleted
                            }).ToList(),

                };
            }
              
        }

        public Result UpdateProfile(Model.StoreDetail.Update value,int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj.ShopId).SingleOrDefault();
                ShopDetail shopdetail = context.ShopDetails.SingleOrDefault(x => x.ShopId == qes);
                if (shopdetail != null)
                {
                    shopdetail.ShopName = value.ShopName;
                    shopdetail.PhoneNumber = value.PhoneNumber;
                    shopdetail.Logo = value.Logo;

                    context.SubmitChanges();
                    return new Result()
                    {

                        Message = string.Format($"Profile Updated Successfully"),
                        Status = Result.ResultStatus.success,
                    };
                }
                else
                {
                    return new Result()
                    {

                        Message = string.Format($"Invalid Shop ID"),
                        Status = Result.ResultStatus.danger,
                    };
                }
            }
                
        }
           
    }
}

