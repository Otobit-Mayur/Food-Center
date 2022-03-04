using FoodCenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.StoreDetails
{
    public class Address
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public async Task<string>Addresss(Model.StoreDetail.Address value)
        {
            //dbo.Address
            FoodCenterContext.Address add = new FoodCenterContext.Address();
            add.Address1 = value.Addresss;
            add.Latitude = value.Latitude;
            add.Longitude = value.Longitude;
            context.Addresses.InsertOnSubmit(add);
            context.SubmitChanges();
            return "Address Added";
        }
    }
}
