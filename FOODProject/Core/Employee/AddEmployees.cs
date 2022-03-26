using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Cores.Employee
{
    public class AddEmployees
    {
        public Result AddEmployee(Model.Employee.AddEmp value, int UserId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (FoodCenterDataContext context = new FoodCenterDataContext())
                {
                    var qes = (from obj in context.OfficeDetails
                               where obj.UserId == UserId
                               select obj.OfficeId).SingleOrDefault();
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



                    EmployeeDetail e = new EmployeeDetail();
                    e.UserId = u.UserId;
                    e.OfficeId = qes;

                    context.EmployeeDetails.InsertOnSubmit(e);
                    context.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = e.EmployeeId,
                    };
                }


            }
        }
        public Result AddEmpPassword(Model.Employee.AddEmployeePassword value, int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserId == Id);
                if (user != null)
                {
                    user.Password = value.Password;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Password Added"),
                        Status = Result.ResultStatus.success,
                        Data = user.UserId,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"User Not Found"),
                    Status = Result.ResultStatus.warning,
                };
            }
        }
        public Result AddEmpDetaile(Model.Employee.EmployeeDetail value,int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                EmployeeDetail e = context.EmployeeDetails.FirstOrDefault(x => x.EmployeeId == Id);
                if (e != null)
                {
                    e.EmployeeName = value.EmployeeName;
                    e.PhoneNumber = value.PhoneNumber;
                    e.Photo = value.Photo;

                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = e.EmployeeId,
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
