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

        public Result UpdateStatus(int Id, Model.Employee.Ordertime value)
        {
            OrderMstr om = db.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
            WalletDetail wallet = db.WalletDetails.FirstOrDefault(x => x.OfficeId == om.OfficeId);
            if (om is null)
            {
                throw new ArgumentException("This Order is not available  ");
            }
            if (om.Track != null)
            {
                throw new ArgumentException("Once you accept the order than you can not cancel that  ");
            }
            if (Id == 0)
            {
                throw new ArgumentException("You have to Give Time For Delivery");
            }
            if (om.Track is null && om.Status == "Approved" && wallet.Balance>=om.Total)
            { 
                var t = value.Time.Id * 10;
                om.Status = "Accepted";
                om.DeliveryTime = DateTime.Now.AddMinutes(t);
                om.Track = "Not Done";
                om.AcceptTime = DateTime.Now;
                wallet.Balance = wallet.Balance - (int)om.Total;
                db.SubmitChanges();

                return new Result()
                {
                    Message = string.Format($"Status Updated Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = om.OrderId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error is Occure");
            }
        }
       
        public Result UpdateTrack(int Id)
        {
            OrderMstr om = db.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
            if (om is null)
            {
                throw new ArgumentException("This Order Is not available  ");
            }
            if (om.Track is null && om.Status != "Accepted")
            {
                throw new ArgumentException("You Are Not Accepted This Order ");
            }
            if (om.Status == "Accepted" && om.Track == "Not Done")
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
                throw new ArgumentException("Some Unknown Error is Occure");
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
                        join e in db.EmployeeDetails
                        on oom.EmployeeId equals e.EmployeeId
                        where oom.ShopId == shop.ShopId && oom.Status == "Approved"
                        select new
                        {
                            OrderId=oom.OrderId,
                            OfficeId = o.OfficeId,
                            OfficeName = o.OfficeName,
                            EmployeeId = e.EmployeeId,
                            EmployeeName = e.EmployeeName,
                            OfficeAddress = oa.AddressLine,
                            Distance = GetDistanceFromShop(p1, o.OfficeId),
                            Image = prod.Image,
                            ItemName = prod.ProductName,
                            ProductType = prod.ProductType.Type,
                            Total=oom.Total,
                          
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
        public Result OrderFilterSort(int UserId,Model.Common.FilterSort Value)
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
                        where oom.ShopId == shop.ShopId && (prod.ProductName.Contains(Value.Filter) || o.OfficeName.Contains(Value.Filter))
                        select new
                        {
                            Image = prod.Image,
                            ItemName = prod.ProductName,
                            Qty = od.Qty,
                            OfficeName = o.OfficeName,
                            OfficeAddress = oa.AddressLine,
                            Distance = GetDistanceFromShop(p1, o.OfficeId)
                        });
            if (Value.SortingOrder == 12)
            {


                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = Value.Sorting == (int)Sort.Quantity ? Res1.OrderByDescending(od => od.Qty).ToList() : Value.Sorting == (int)Sort.Distance ? Res1.OrderByDescending(od => od.Distance).ToList() : Res1,
                };
            }
            else if (Value.SortingOrder == 13)
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = Value.Sorting == (int)Sort.Quantity ? Res1.OrderBy(od => od.Qty).ToList() : Value.Sorting == (int)Sort.Distance ? Res1.OrderBy(od => od.Distance).ToList() : Res1,
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
        public Result TodaysOrder(int UserId)
        {
            var shop = (from sp in db.ShopDetails
                        where sp.UserId == UserId
                        select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }  
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from oom in db.OrderMstrs
                        join od in db.OrderDetails
                        on oom.OrderId equals od.OrderId
                        where Convert.ToDateTime(od.OrderTime).Date == DateTime.Now.Date && oom.ShopId == shop.ShopId && oom.Status == "Approved"
                        select oom).Count(),
            };

        }
        public Result TodaysDeliveredOrder(int UserId)
        {
            var shop = (from sp in db.ShopDetails
                        where sp.UserId == UserId
                        select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from oom in db.OrderMstrs
                        join od in db.OrderDetails
                        on oom.OrderId equals od.OrderId
                        where Convert.ToDateTime(od.OrderTime).Date == DateTime.Now.Date && oom.ShopId == shop.ShopId && oom.Track == "Done"
                        select oom).Count(),
            };

        }
        public Result GetTopOrder(int UserId)
        {
            var shop = (from sp in db.ShopDetails
                        where sp.UserId == UserId
                        select sp).FirstOrDefault();
            if (shop is null)
            {
                throw new ArgumentException("Shop not found!");
            }
            // var items = db.OrderDetails.SelectMany(o => o.ProductId);
            //var products = (from product in db.Products
            //                join order in db.OrderDetails 
            //                on product.ProductId equals order.ProductId into matchingItems
            //                orderby matchingItems.Sum(oi => oi.Qty)
            //                select new
            //                {
            //                    Image = product.Image,
            //                    ItemName = product.ProductName,
            //                }).Take(5);
            var products = (from order in db.OrderDetails
                            group order by order.ProductId into ans
                            orderby ans.Key descending
                            select new
                            {
                                ProductId = ans.Key,
                                Order = ans.OrderBy(x=>x.OrderDetailId)
                            });
           
            foreach (var group in products)
            {
                Console.WriteLine(group.ProductId + " : " + group.Order.Count());
                foreach (var o in group.Order)
                {
                    Console.WriteLine("  OrderId :" + o.OrderDetailId );
                }
            }
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = products,
            };

        }
  
    }
}
//from l in logins
//group l by l.Date into g
//select new
//{
//    Date = g.Key,
//    Count = (from l in g select l.Login).Distinct().Count()
//};
//For a side by side comparison to the original method syntax (which personally I like better) here you go...

//logins
//  .GroupBy(l => l.Date)
//  .Select(g => new
//   {
//       Date = g.Key,
//       Count = g.Select(l => l.Login).Distinct().Count()
//   });