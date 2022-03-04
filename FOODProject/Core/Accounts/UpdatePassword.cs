using FoodCenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Accounts
{
    
    public class UpdatePassword
    {
        FoodCenterDataContext context = new FoodCenterDataContext();
        public async Task<string> Changepassword(Model.Account.Changepassword value, int id)
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
                    return "Password Changed";
                }
                else
                {
                    return "Old Password is Incorrect";
                }
            }
            return "User Not Found";

        }
    }
}
