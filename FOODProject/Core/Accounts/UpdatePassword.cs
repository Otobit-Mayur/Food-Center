using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Accounts
{
    
    public class UpdatePassword
    {
        FoodCenterDataContext context = new FoodCenterDataContext();
        public Result Changepassword(Model.Account.Changepassword value, int id)
        {
            User user = context.Users.SingleOrDefault(x => x.UserId == id);
            if (user != null)
            {
                user.Password = value.OldPassword;
                var check = context.Users.SingleOrDefault(x => x.Password == value.OldPassword);
                if(check!=null)
                {
                    user.Password = value.NewPassword;
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
