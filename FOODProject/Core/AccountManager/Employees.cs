using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager
{
    public class Employees
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result GetAllEmployee(int UserId)
        {
            var office = (from obj in context.OfficeDetails
                          where obj.UserId == UserId
                          select obj).SingleOrDefault();
            if (office is null)
            {
                throw new ArgumentException("Office not found!");
            }
            if (office != null)
            {
                var count = (from e in context.EmployeeDetails
                             where e.OfficeId == office.OfficeId
                             select e).Count();
                if (count == 0)
                {
                    return new Result()
                    {
                        Message = string.Format($"You Dont Have Any Employee"),
                        Status = Result.ResultStatus.success,
                        Data = office.OfficeId,
                    };
                }
                else
                {
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Get All Detail Successfully"),
                        Data = (from e in context.EmployeeDetails
                                where e.OfficeId == office.OfficeId
                                select new { 
                                    EmployeeId = e.EmployeeId,
                                    EmployeeName = e.EmployeeName,
                                    Photo = e.Photo,

                        }).ToList(),
                    };
                }
               
            }
            else
            {
                throw new ArgumentException("Some Unknown Error Is Occure");
            }
        }
        public Result DeleteEmployee(int Id)
        {
            EmployeeDetail e = context.EmployeeDetails.FirstOrDefault(x => x.EmployeeId == Id);
            if (e is null)
            {
                throw new ArgumentException("Employee Not Found");
            }
            if (e.IsDeleted == "True")
            {
                throw new ArgumentException("Employee Already Deleted");
            }

            if (e.IsDeleted == "False")
            {
                e.IsDeleted = "True";
                e.IsActive = "False";
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Employee Deleted Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = e.EmployeeId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error is Occure");
            }
        }
        public Result EmployeeStatus(int Id)
        {
            EmployeeDetail e = context.EmployeeDetails.FirstOrDefault(x => x.EmployeeId == Id);
            if (e is null)
            {
                throw new ArgumentException("Employee Not Found");
            }
            if (e.IsDeleted == "True")
            {
                throw new ArgumentException("Employee is Deleted");
            }

            if (e.IsActive == "True")
            {
                e.IsActive = "False";
            }
            else
            {
                e.IsActive = "True";
            }
            context.SubmitChanges();
            return new Result()
            {
                Message = string.Format($"Employee Status Update Successfully"),
                Status = Result.ResultStatus.success,
                Data = e.EmployeeId,
            };
        }
    }
}
