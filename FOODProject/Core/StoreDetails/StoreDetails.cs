﻿using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Accounts
{
    public class StoreDetails
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result AddStoreDetails(Model.StoreDetail.StoreDetail value)
        {
            ShopDetail sd = new ShopDetail();

            sd.IsCompleted = 0;
            sd.ShopName = value.ShopName;
            sd.PhoneNumber = value.PhoneNumber;
            sd.AlternateNumber = value.AlternateNumber;
            sd.DeliveryRadius =value.DeliveryRadius;
            sd.Logo = value.Logo;
            sd.Status = 1;

            var ci = context.Categories.SingleOrDefault(c => c.Category1 == value.CategoryId.String);

            var ui = context.Users.SingleOrDefault(c=>c.EmailId==value.UserId.String);

            
            sd.CategoryId = ci.CategoryId;
            sd.UserId = ui.UserId;
           

            context.ShopDetails.InsertOnSubmit(sd);
            context.SubmitChanges();
            return new Result()
            { 

                Message = string.Format($"Details Added Successfully"),
                Status = Result.ResultStatus.success,
                Data=sd.ShopId,
            };

        }
        public Result getallShop()
        {
            return new Result()
            {
                Message = String.Format($"Get All Shop Details"),
                Status = Result.ResultStatus.success,
                Data = (from obj in context.ShopDetails
                        select new { obj.ShopId, obj.ShopName, obj.PhoneNumber,
                                     obj.DeliveryRadius,obj.UserId,obj.CategoryId,
                                     obj.Status,obj.IsCompleted }).ToList(),

            };
        }

        public Result UpdateProfile(Model.StoreDetail.Update value,int UserId)
        {
            var qes = (from obj in context.ShopDetails
                       where obj.UserId == UserId
                       select obj.ShopId).SingleOrDefault();
            ShopDetail shopdetail = context.ShopDetails.SingleOrDefault(x=>x.ShopId==qes);
            if(shopdetail !=null)
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
