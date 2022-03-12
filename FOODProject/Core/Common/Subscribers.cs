using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Common
{
    public class Subscribers
    {
        FoodCenterDataContext db = new FoodCenterDataContext();
        OfficeDetail of = new OfficeDetail();

        public Result GetAllSubscriber(int UserId)
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
                        where s.Status == 1 && s.ShopId == sid
                        select new
                        {
                            Image = off.Image,
                            Name = off.Name,
                            Address = oa.Address,
                            Staff = off.Staff,
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
                                Name = off.Name,
                                Address = oa.Address,
                                Staff = off.Staff,
                                Phone = off.Phone,
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
                        where s.Status == 0 && s.ShopId == sid
                        select new
                        {
                            Image = off.Image,
                            Name = off.Name,
                            Address = oa.Address,
                            Staff = off.Staff
                        }).ToList(),
            };
        }
        public Result GetLocation(int UserId, string EmailId, int Id)
        {
            OfficeDetail of = db.OfficeDetails.SingleOrDefault(x => x.OfficeId == Id);
            if (of != null)
            {
                var sid = (from sp in db.ShopDetails
                           where sp.UserId == UserId
                           select sp.ShopId).SingleOrDefault();

                var p1 = (from sp in db.ShopDetails
                                 join a in db.Addresses
                                 on sp.ShopId equals a.ShopId
                                 where a.ShopId == sid
                                 select a).SingleOrDefault();
               
                var p2 = (from od in db.OfficeDetails
                                 join oa in db.OfficeAddresses
                                 on od.OfficeId equals oa.OfficeId
                                 where oa.OfficeId == Id
                                 select oa).SingleOrDefault();
                
                var d1 = (double)p1.Latitude * (Math.PI / 180.0);
                var num1 = (double)p1.Longitude * (Math.PI / 180.0);
                var d2 = (double)p2.Latitude * (Math.PI / 180.0);
                var num2 = (double)p2.Longitude * (Math.PI / 180.0) - num1;
                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                         Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
                var loc = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
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
                            where s.Status == 1 && s.ShopId == sid && off.OfficeId == Id
                            select new
                            {
                                Image = off.Image,
                                Name = off.Name,
                                Address = oa.Address,
                                Staff = off.Staff,
                                Phone = off.Phone,
                                Email = EmailId,
                                Distance = loc
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
        
    }
    
}
