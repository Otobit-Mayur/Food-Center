using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager
{
    public class OfficesHome
    {
        public Result GetOfficeDetail(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.OfficeDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if (qes == null)
                {
                    throw new ArgumentException("Office Id not Found");
                }
                return new Result()
                {
                    Message = string.Format($"Get Detail Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.OfficeDetails
                            join add in context.OfficeAddresses
                            on obj.OfficeId equals add.OfficeId
                            where obj.OfficeId == qes.OfficeId
                            select new
                            {
                                Logo = obj.Image,
                                ManagerName = obj.ManagerName,
                                OffeName=obj.OfficeName,
                                Address = add.AddressLine,
                            }).ToList(),
                };
            }
        }
        public Result GetCategory()
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
                return new Result()
                {
                    Message = string.Format($"Get Category Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.Categories
                            select new { obj.CategoryId, obj.CategoryName }).ToList(),
                };
            }
        }
    }
}
