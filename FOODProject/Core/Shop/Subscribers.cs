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
        OfficeDetail of = new OfficeDetail();

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
                        where s.Status == 1 && s.ShopId == shop.ShopId
                        select new
                        {
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
                            where s.Status == 1 && s.ShopId == sid && s.OfficeId == Id
                            select new
                            {
                                Image = off.Image,
                                Name = off.OfficeName,
                                Address = oa.AddressLine,
                                Staff = off.Staff,
                                Phone = off.PhoneNumber,
                                Email = u.EmailId
                            }).ToList(),
                };
            }
            else
            {
                return new Result()
                {
                    Message = string.Format("Data not available  "),
                    Status = Result.ResultStatus.warning,
                };
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
                        where s.Status == 0 && s.ShopId == shop.ShopId
                        select new
                        {
                            Image = off.Image,
                            Name = off.OfficeName,
                            Address = oa.AddressLine,
                            Staff = off.Staff,
                            Distance = GetDistanceFromShop(p1, off.OfficeId)
                        }).ToList(),
            };
        }
        public double GetDistanceFromShop(ShopAddress p1 ,int OfficeId ) {
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
            if (of != null)
            {
                var sid = (from sp in db.ShopDetails
                           where sp.UserId == UserId
                           select sp.ShopId).SingleOrDefault();
                
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = (from om in db.OrderMstrs
                            join od in db.OrderDetails
                            on om.OrderId equals od.OrderId
                            join p in db.Products
                            on od.ProductId equals p.ProductId
                            where om.Status == "Accepted" && om.Track == "Done" && om.ShopId == sid && om.OfficeId == Id
                            orderby om.DeliveryTime
                            select new
                            {
                                ProductName = p.ProductName,
                                Qty = od.Qty,
                                Total = om.Total,
                                Time = om.DeliveryTime                               
                            }
                    ).ToList(),
                };
            }
            else
            {
                return new Result()
                {
                    Message = string.Format("Data not available  "),
                    Status = Result.ResultStatus.warning,
                };
            }
        }
    }
    
}
