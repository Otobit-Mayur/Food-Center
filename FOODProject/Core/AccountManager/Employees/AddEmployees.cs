using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager.Employees
{
    public class AddEmployees
    {
        public Result AddEmployee(Model.AccountManager.Employee.AddEmp value)
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
                User u = new User();
                u.EmailId = value.EmailId;
                u.RoleId = 3;
                var res = context.Users.FirstOrDefault(x => x.EmailId == value.EmailId);
                if (res != null)
                {
                    throw new ArgumentException("Email Id Already Exist");
                }
                else
                { 
                context.Users.InsertOnSubmit(u);
                context.SubmitChanges();
                }
                return new Result()
                {
                    Message = string.Format($"{value.EmailId} Signup Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = u.UserId,
                };
            }
        }
        public Result AddEmpPassword(Model.AccountManager.Employee.AddEmployeePassword value,int Id)
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserId == Id);
                if(user!=null)
                {
                    user.Password = value.Password;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Password Added"),
                        Status = Result.ResultStatus.success,
                        Data=user.UserId,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"User Not Found"),
                    Status = Result.ResultStatus.warning,
                };
            }
        }
    }
}
