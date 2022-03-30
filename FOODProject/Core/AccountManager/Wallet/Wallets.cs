using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager.Wallet
{
    public class Wallets
    {
        public Result GetWallet(int UserId)
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
              
                var qes = (from obj in context.OfficeDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if (qes is null)
                {
                    throw new ArgumentException("Office Not Found!");
                }

                return new Result()
                {
                    Message = string.Format($"Get Wallet Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.WalletDetails
                            where obj.OfficeId==qes.OfficeId
                            select new
                            {
                                Balance=obj.Balance,
                            }).ToList(),
                };
            }
        }
        public Result AddBalance(Model.AccountManager.Wallet.Wallet value,int UserId)
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
                var qes = (from obj in context.OfficeDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if (qes is null)
                {
                    throw new ArgumentException("Office Not Found!");
                }

                //Amount = Amount + (int)value.AddAmount;
                //var Amount = 0;
                WalletDetail wallet = context.WalletDetails.FirstOrDefault(x => x.OfficeId == qes.OfficeId);
                if(wallet.OfficeId is not null)
                {
                    
                    wallet.Balance = wallet.Balance + value.AddAmount;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Balance Added"),
                        Status = Result.ResultStatus.success,
                        Data=wallet.Balance,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"Invalid User"),
                    Status = Result.ResultStatus.success,
                };


            }
        }
    }
}
