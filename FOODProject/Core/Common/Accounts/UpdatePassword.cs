using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Common.Accounts
{
    
    public class UpdatePassword
    {
        FoodCenterDataContext context = new FoodCenterDataContext();
        public Result Changepassword(Model.Common.Account.Changepassword value, int Id)
        {
            User user = context.Users.FirstOrDefault(x => x.UserId == Id);
            if(user==null)
            {
                throw new ArgumentException("User Is Invalid");
            }
            else
            {
                    var check = (from obj in context.Users
                                 where obj.UserId==Id &&  obj.Password == value.OldPassword
                                 select obj).SingleOrDefault();
                    if (check != null)
                    {
                        check.Password = value.NewPassword;
                        context.SubmitChanges();
                        return new Result()
                        {
                            Message = string.Format("Password Change Successfully"),
                            Status = Result.ResultStatus.success,
                        };
                    }
                    else
                    {
                        return new Result()
                        {
                            Message = string.Format("Old Password Is Incorrect"),
                            Status = Result.ResultStatus.warning,
                        };
                    }
                
            }
            return new Result()
            {
                Message = string.Format("User Not Found"),
                Status = Result.ResultStatus.none,
            };

        }
    }
}
