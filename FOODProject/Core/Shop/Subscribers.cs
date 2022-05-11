using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop
{
    public class Subscribers
    {
        FoodCenterDataContext db = new FoodCenterDataContext();

        public Result GetAllSubscriber(int UserId)
        {
            var shop = (from sp in db.ShopDetails
                        where sp.UserId == UserId
                        select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            var p1 = (from x in db.ShopAddresses where x.ShopId == shop.ShopId select x).FirstOrDefault();

            if (p1 is null)
            {
                throw new ArgumentException("Shop Address not found!");
            }
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from s in db.Subscribers
                        join off in db.OfficeDetails
                        on s.OfficeId equals off.OfficeId
                        join oa in db.OfficeAddresses
                        on off.OfficeId equals oa.OfficeId
                        where s.Subscription == "Accepted" && s.ShopId == shop.ShopId
                        select new
                        {
                            OfficeId = off.OfficeId,
                            Image = off.Image,
                            Name = off.OfficeName,
                            Address = oa.AddressLine,
                            Staff = off.Staff,
                            Distance = GetDistanceFromShop(p1, off.OfficeId)
                        }).ToList(),
            };
        }
        public Result GetById(int UserId, int Id)
        {
            OfficeDetail of = db.OfficeDetails.SingleOrDefault(x => x.OfficeId == Id);
            if(of is null)
            {
                throw new ArgumentException("Office Is Not Found!");
            }
            if (of != null)
            {
                var sid = (from sp in db.ShopDetails
                           where sp.UserId == UserId          
                           select sp.ShopId).SingleOrDefault();
                
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = (from s in db.Subscribers
                            join off in db.OfficeDetails
                            on s.OfficeId equals off.OfficeId
                            join sp in db.ShopDetails
                            on s.ShopId equals sp.ShopId
                            join oa in db.OfficeAddresses
                            on off.OfficeId equals oa.OfficeId
                            join u in db.Users
                            on off.UserId equals u.UserId
                            where s.Subscription == "Accepted" && s.ShopId == sid && s.OfficeId == Id
                            select new
                            {
                                OfficeId = off.OfficeId,
                                Image = off.Image,
                                Name = off.OfficeName,
                                Address = oa.AddressLine,
                                Staff = off.Staff,
                                Phone = off.PhoneNumber,
                                Email = u.EmailId
                            }).FirstOrDefault(),
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error Is Occure");
            }
        }
        
        public Result GetAllRequest(int UserId)
        {
            var shop = (from sp in db.ShopDetails
                       where sp.UserId == UserId
                       select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            var p1 = (from x in db.ShopAddresses where x.ShopId == shop.ShopId select x).FirstOrDefault();

            if (p1 is null)
            {
                throw new ArgumentException("Shop Address not found!");
            }
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from s in db.Subscribers
                        join off in db.OfficeDetails
                        on s.OfficeId equals off.OfficeId
                        join oa in db.OfficeAddresses
                        on off.OfficeId equals oa.OfficeId
                        where s.Subscription == "Not Accepted" && s.ShopId == shop.ShopId
                        select new
                        {
                            SubscriberId=s.SubscriberId,
                            OfficeId=off.OfficeId,
                            Image = off.Image,
                            Name = off.OfficeName,
                            Address = oa.AddressLine,
                            Staff = off.Staff,
                            Distance = GetDistanceFromShop(p1, off.OfficeId)
                        }).ToList(),
            };
        }
        public Result ApproveRequest(int Id)
        {
            Subscriber s = db.Subscribers.FirstOrDefault(x => x.SubscriberId == Id);
            if(s!=null)
            {
                if (s.Subscription == "Not Accepted")
                {
                    s.Subscription = "Accepted";
                    s.Status = true;
                    db.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Request Accepted Successfully "),
                        Data=s.SubscriberId,
                    };
                }
                else
                {
                    return new Result()
                    {
                        Status = Result.ResultStatus.danger,
                        Message = string.Format("Request Already Accepted"),
                        Data = s.SubscriberId,
                    };
                }
            }

            return new Result()
            {
                Status = Result.ResultStatus.danger,
                Message = string.Format("Id Not Found"),
            };

        }
        public Result RejectRequest(int Id)
        {
            Subscriber s = db.Subscribers.FirstOrDefault(x => x.SubscriberId == Id);
            if (s != null)
            {
                if (s.Subscription == "Not Accepted")
                {
                    s.Subscription = "Rejected";
                    db.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Request Rejected Successfully "),
                        Data = s.SubscriberId,
                    };
                }
                else
                {
                    return new Result()
                    {
                        Status = Result.ResultStatus.danger,
                        Message = string.Format("Request Already Rejected"),
                        Data = s.SubscriberId,
                    };
                }
            }

            return new Result()
            {
                Status = Result.ResultStatus.danger,
                Message = string.Format("Id Not Found"),
            };

        }
        public double GetDistanceFromShop(ShopAddress p1 ,int OfficeId ) 
        {
            var p2 = (from x in db.OfficeAddresses where x.OfficeId == OfficeId select x).SingleOrDefault();

            if(p2.Latitude is null || p2.Longitude is null)
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
        public Result GetResentOrder(int UserId, int Id)
        {
            OfficeDetail of = db.OfficeDetails.SingleOrDefault(x => x.OfficeId == Id);
            if(of is null)
            {
                throw new ArgumentException("Office Is Not Found");
            }
            if (of != null)
            {
                var sid = (from sp in db.ShopDetails
                           where sp.UserId == UserId
                           select sp.ShopId).SingleOrDefault();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = (from od in db.OrderDetails
                            where od.OrderMstr.Track == "Done" && od.OrderMstr.ShopId == sid && od.OrderMstr.OfficeId == Id
                            select new
                            {
                                ProductId = od.Product.ProductId,
                                ProductName = od.Product.ProductName,
                                ImageId = od.Product.ImageId,
                                Path = od.Product.ImageId == 0 ? null : od.Product.Image.Path,
                                Qty = od.Qty,
                                Total = od.OrderMstr.Total,
                                Time = od.OrderMstr.DeliveryTime
                            }).ToList(),
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error Is Occured");
            }
        }
        public Result CancelSubscription(int Id, int UserId)
        {
            OfficeDetail office = db.OfficeDetails.FirstOrDefault(x => x.OfficeId == Id);
            if (office is null)
            {
                throw new ArgumentException("Office Not Found!");
            }
            var shop = (from obj in db.ShopDetails
                          where obj.UserId == UserId
                          select obj).SingleOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            var sub = (from s in db.Subscribers
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
                sub.Subscription = "Cancel By Shop";
                db.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Cancel Subscription Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = office.OfficeId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error Occure");
            }

        }
    }
    
}
