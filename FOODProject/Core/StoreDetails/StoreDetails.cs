using FoodCenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Accounts
{
    public class StoreDetails
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public async Task<string> AddStoreDetails(Model.StoreDetail.StoreDetail value)
        {
            ShopDetail sd = new ShopDetail();
           
            sd.ShopName = value.ShopName;
            sd.PhoneNumber = value.PhoneNumber;
            sd.AlternateNumber = value.AlternateNumber;
            sd.DeliveryRadius =value.DeliveryRadius;
            sd.Logo = value.Logo;
            sd.Status = 1;


            var ci = context.Categories.SingleOrDefault(c => c.Category1 == value.CategoryId.String);

            var ui = context.Users.SingleOrDefault(c=>c.EmailId==value.UserId.String);

            var ai = context.Addresses.SingleOrDefault(c => c.Address1 == value.AddressId.String);

            
            sd.CategoryId = ci.CategoryId;
            sd.UserId = ui.UserId;
            sd.AddressId = ai.AddressId;

            context.ShopDetails.InsertOnSubmit(sd);
            context.SubmitChanges();
            return "Details Added Successfully";

        }

        public async Task<string> UpdateProfile(Model.StoreDetail.Update value,int id)
        {
            ShopDetail shopdetail = context.ShopDetails.SingleOrDefault(x=>x.ShopId==id);
            if(shopdetail !=null)
            {
                shopdetail.ShopName = value.ShopName;
                shopdetail.PhoneNumber = value.PhoneNumber;
                shopdetail.Logo = value.Logo;
       
                context.SubmitChanges();
                return "Profile Updated";
            }
            else
            {
                return "Invalid Shop Id";
            }
        }
           
    }
}
