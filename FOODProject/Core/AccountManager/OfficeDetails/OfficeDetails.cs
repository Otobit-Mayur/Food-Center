using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager.OfficeDetails
{
    public class OfficeDetails
    {
        

        public Result AddOfficeDetail(Model.AccountManager.OfficeDetail.OfficeDetail value)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                OfficeDetail OD = new OfficeDetail();

                OD.OfficeName = value.OfficeName;
                OD.ManagerName = value.OfficeName;
                OD.PhoneNumber = value.PhoneNumber;
                OD.AlternateNumber = value.PhoneNumber;
                OD.Image = value.Image;
                var ui = context.Users.SingleOrDefault(c => c.EmailId == value.UserId.String);
                var check = context.OfficeDetails.SingleOrDefault(c => c.UserId == ui.UserId);
                if (check != null)
                {
                    throw new ArgumentException("Entered Email Already Registered for another store");
                }

                OD.UserId = ui.UserId;
                context.OfficeDetails.InsertOnSubmit(OD);
                context.SubmitChanges();

                FoodCenterContext.OfficeAddress add = new OfficeAddress()
                {
                    Address = value.Address.AddresssLine,
                    Latitude=value.Address.Latitude,
                    Longitude=value.Address.Longitude,
                    OfficeId=OD.OfficeId,
                };
                context.OfficeAddresses.InsertOnSubmit(add);
                context.SubmitChanges();

                return new Result()
                {

                    Message = string.Format($"Details Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = OD.OfficeId,
                };
            }
        }
        public Result GetAllOfficeDetail()
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                return new Result()
                {
                    Message = String.Format($"Get All Office Details"),
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.OfficeDetails
                            join add in context.OfficeAddresses
                            on obj.OfficeId equals add.OfficeId
                            select new { obj.OfficeId, obj.OfficeName, obj.ManagerName, obj.PhoneNumber, obj.Image, obj.UserId,add.Address,add.Latitude,add.Longitude }).ToList(),
                };
            }
        }
    }
}
