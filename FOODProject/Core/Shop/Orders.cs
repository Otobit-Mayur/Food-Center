using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop
{
    public class Orders
    {

        FoodCenterDataContext db = new FoodCenterDataContext();
        OrderDetail od = new OrderDetail();
        OrderMstr om = new OrderMstr();

        public Result UpdateStatus(int Id, Model.Shop.Order value)
        {
            OrderMstr om = db.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
            if (om != null)
            {
                if (om.Track == null)
                {
                    var t = value.Time.Id * 10;
                    om.Status = "Accepted";
                    om.DeliveryTime = DateTime.Now.AddMinutes(t);
                    om.Track = "Not Done";
                    om.AcceptTime = DateTime.Now;
                    db.SubmitChanges();

                    return new Result()
                    {
                        Message = string.Format("Status Updated Successfully"),
                        Status = Result.ResultStatus.success,
                    };
                }
                else
                {
                    return new Result()
                    {
                        Message = string.Format("Once you accept the order than you can not cancel that  "),
                        Status = Result.ResultStatus.warning,
                    };
                }
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
        public Result UpdateTrack(int Id)
        {
            OrderMstr om = db.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
            if (om != null)
            {
                if (om.Status == "Accepted")
                {
                    om.Track = "Done";
                    om.AcceptTime = DateTime.Now;
                    db.SubmitChanges();

                    return new Result()
                    {
                        Message = string.Format("Track Updated Successfully"),
                        Status = Result.ResultStatus.success,
                    };
                }
                else
                {
                    return new Result()
                    {
                        Message = string.Format("You Are Not Accepted This Order "),
                        Status = Result.ResultStatus.warning,
                    };
                }
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
        public Result GetAllOrder(int UserId)
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
                Data = (from oom in db.OrderMstrs
                        join od in db.OrderDetails
                        on oom.OrderId equals od.OrderId
                        join prod in db.Products
                        on od.ProductId equals prod.ProductId
                        join o in db.OfficeDetails
                        on oom.OfficeId equals o.OfficeId
                        join oa in db.OfficeAddresses
                        on o.OfficeId equals oa.OfficeId
                        where oom.ShopId == shop.ShopId 
                        select new
                        {
                            Image = prod.Image,
                            ItemName = prod.ProductName,
                            Qty = od.Qty,
                            OfficeName = o.OfficeName,
                            OfficeAddress = oa.AddressLine,
                            Distance = GetDistanceFromShop(p1, o.OfficeId)
                        }).ToList(),
            };

        }
        public double GetDistanceFromShop(ShopAddress p1, int OfficeId)
        {
            var p2 = (from x in db.OfficeAddresses where x.OfficeId == OfficeId select x).FirstOrDefault();

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
       
        public Result GetOrderHistory(int UserId)
        {
            var shop = (from sp in db.ShopDetails
                        where sp.UserId == UserId
                        select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            var p1 = (from x in db.OrderMstrs where x.ShopId == shop.ShopId select x).FirstOrDefault();

            if (p1 is null)
            {
                throw new ArgumentException("No Order Found!");
            }
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from om in db.OrderMstrs
                        join od in db.OrderDetails
                        on om.OrderId equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.ProductId
                        where om.Status == "Accepted" && om.Track == "Done" && om.ShopId == shop.ShopId
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
        public Result GetTime()
        {
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from l in db.LookUps
                        join fl in db.FixLookUps
                        on l.LookUpId equals fl.LookUpId
                        where fl.LookUpId == 1
                        select fl).ToList(),
            };
        }
        public Result GetStatus()
        {
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from l in db.LookUps
                        join fl in db.FixLookUps
                        on l.LookUpId equals fl.LookUpId
                        where fl.LookUpId == 2
                        select fl).ToList(),
            };
        }
        public Result GetTrack()
        {
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from l in db.LookUps
                        join fl in db.FixLookUps
                        on l.LookUpId equals fl.LookUpId
                        where fl.LookUpId == 3
                        select fl).ToList(),
            };
        }
        public Result OrderFilterSort(int UserId,string Filter,int Sorting,int SortingOrder)
        {
            var shop = (from sp in db.ShopDetails
                        where sp.UserId == UserId
                        select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            var p1 = (from x in db.ShopAddresses where x.ShopId == shop.ShopId select x).FirstOrDefault();

            var Res1 = (from oom in db.OrderMstrs
                          join od in db.OrderDetails
                          on oom.OrderId equals od.OrderId
                          join prod in db.Products
                          on od.ProductId equals prod.ProductId
                          join o in db.OfficeDetails
                          on oom.OfficeId equals o.OfficeId
                          join oa in db.OfficeAddresses
                          on o.OfficeId equals oa.OfficeId
                          where oom.ShopId == shop.ShopId && (prod.ProductName.Contains(Filter) || o.OfficeName.Contains(Filter))
                          select new
                          {
                              Image = prod.Image,
                              ItemName = prod.ProductName,
                              Qty = od.Qty,
                              OfficeName = o.OfficeName,
                              OfficeAddress = oa.AddressLine,
                              Distance = GetDistanceFromShop(p1, o.OfficeId)
                          });
            if (SortingOrder == 12)
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = Res1.OrderByDescending(od=>od.Qty).ToList(),
                };
            }
            else if (SortingOrder == 13)
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = Res1.OrderBy(od=>od.Qty).ToList(),
                };
            }
            else
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = Res1
                };

            }
        }
    }
}


