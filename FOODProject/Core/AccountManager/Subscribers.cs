using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager
{
    public class Subscribers
    {
        FoodCenterDataContext context = new FoodCenterDataContext();
        public Result GetAllSubscriber(int UserId)
        {
            var office = (from obj in context.OfficeDetails
                       where obj.UserId == UserId
                       select obj).SingleOrDefault();
            if (office is null)
            {
                throw new ArgumentException("Office not found!");
            }
            var sub = (from x in context.Subscribers where x.OfficeId == office.OfficeId select x).FirstOrDefault();
            if (sub is null)
            {
                throw new ArgumentException("Subscription not Done!");
            }
            var p1 = (from x in context.OfficeAddresses where x.OfficeId == office.OfficeId select x).FirstOrDefault();

            if (p1 is null)
            {
                throw new ArgumentException("Office Address not found!");
            }
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from sd in context.ShopDetails
                        join sa in context.ShopAddresses
                        on sd.ShopId equals sa.ShopId
                        join s in context.Subscribers
                        on sd.ShopId equals s.ShopId
                        where s.Subscription == "Accepted" && s.OfficeId==office.OfficeId
                        select new
                        {
                            ShopId=s.ShopId,
                            ShopImage = sd.Logo,
                            ShopName = sd.ShopName,
                            ShopAddress = sa.AddressLine,
                            Status=s.Status,
                            Distance = GetDistanceFromShop(p1, sa.AddressId)
                        }).ToList(),
            };
        }
        public double GetDistanceFromShop(OfficeAddress p1, int OfficeId)
        {
            var p2 = (from x in context.ShopAddresses where x.ShopId == OfficeId select x).FirstOrDefault();

            if (p2.Latitude is null || p2.Longitude is null)
            {
                return 0.0;
            }

            var d1 = (double)p1.Latitude * (Math.PI / 180.0);
            var num1 = (double)p1.Longitude * (Math.PI / 180.0);
            var d2 = (double)p2.Latitude * (Math.PI / 180.0);
            var num2 = (double)p2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            var loc = Math.Round(6371.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
            return loc;
        }
        public Result UpdateStatus(int Id,int UserId)
        {
            ShopDetail shop = context.ShopDetails.FirstOrDefault(x => x.ShopId == Id);
            if (shop is null)
            {
                throw new ArgumentException("Shop Not Found!");
            }
            var office = (from obj in context.OfficeDetails
                          where obj.UserId == UserId
                          select obj).SingleOrDefault();
            if (office is null)
            {
                throw new ArgumentException("Office not found!");
            }
            var sub = (from s in context.Subscribers
                       where s.ShopId == shop.ShopId && s.OfficeId == office.OfficeId
                       select s).FirstOrDefault();
            if (sub is null)
            {
                throw new ArgumentException("Subscription Not Done!");
            }

            if (sub.Status == true)
            {
                sub.Status = false;
            }
            else
            {
                sub.Status = true;
            }
            context.SubmitChanges();
            return new Result()
            {
                Message = string.Format($"Status Updated Successfully"),
                Status = Result.ResultStatus.success,
                Data = shop.ShopId,
            };
        }
        public Result CancelSubscription(int Id, int UserId)
        {
            ShopDetail shop = context.ShopDetails.FirstOrDefault(x => x.ShopId == Id);
            if (shop is null)
            {
                throw new ArgumentException("Shop Not Found!");
            }
            var office = (from obj in context.OfficeDetails
                          where obj.UserId == UserId
                          select obj).SingleOrDefault();
            if (office is null)
            {
                throw new ArgumentException("Office not found!");
            }
            var sub = (from s in context.Subscribers
                       where s.ShopId == shop.ShopId && s.OfficeId == office.OfficeId
                       select s).FirstOrDefault();
            if (sub is null)
            {
                throw new ArgumentException("There Is No Subscription!");
            }
            if (sub.Subscription == "Cancel By Shop" || sub.Subscription == "Cancel By Office")
            {
                throw new ArgumentException("Subscription is Already Cancel!");
            }
            if (sub.Subscription == "Accepted")
            {
                sub.Subscription = "Cancel By Office";
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Status Updated Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = shop.ShopId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error Occure");
            }
           
        }
    }
}
