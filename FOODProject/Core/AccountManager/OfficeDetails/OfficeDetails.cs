﻿using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager.OfficeDetails
{
    public class OfficeDetails
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result AddOfficeDetail(Model.AccountManager.OfficeDetail.OfficeDetail value)
        {
            OfficeDetail OD = new OfficeDetail();
            OD.OfficeName = value.OfficeName;
            OD.ManagerName = value.OfficeName;
            OD.PhoneNumber = value.PhoneNumber;
            OD.AlternateNumber = value.PhoneNumber;
            OD.Image = value.Image;
            var ui = context.Users.SingleOrDefault(c => c.EmailId == value.UserId.String);
            OD.UserId = ui.UserId;
            context.OfficeDetails.InsertOnSubmit(OD);
            context.SubmitChanges();
            return new Result()
            {

                Message = string.Format($"Details Added Successfully"),
                Status = Result.ResultStatus.success,
                Data = OD.OfficeId,
            };
        }
        public Result GetAllOfficeDetail()
        {
            return new Result()
            {
                Message = String.Format($"Get All Office Details"),
                Status = Result.ResultStatus.success,
                Data = (from obj in context.OfficeDetails
                        select new { obj.OfficeId, obj.OfficeName, obj.ManagerName, obj.PhoneNumber, obj.Image, obj.UserId }).ToList(),
            };
        }
    }
}