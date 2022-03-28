using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Core.Employee
{
    public class Orders
    {
        public Result GetAllSubscriber(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var emp = (from obj in context.EmployeeDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if (emp is null)
                {
                    throw new ArgumentException("Employee not found!");
                }

                var sub = (from x in context.Subscribers where x.OfficeId == emp.OfficeId select x).FirstOrDefault();
                if (sub is null)
                {
                    throw new ArgumentException("Subscription not Done!");
                }

                var p1 = (from x in context.OfficeAddresses where x.OfficeId == emp.OfficeId select x).FirstOrDefault();

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
                            where s.Subscription == "Accepted" && s.Status == "ON"
                            select new
                            {
                                ShopImage = sd.Logo,
                                ShopName = sd.ShopName,
                                ShopAddress = sa.AddressLine,
                                Distance = GetDistanceFromShop(p1, sa.AddressId)
                            }).ToList(),
                };
            }
        }
        public double GetDistanceFromShop(OfficeAddress p1, int OfficeId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
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
        }
        public Result GetAllProduct(int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var shop = (from x in context.ShopDetails where x.ShopId == Id select x).FirstOrDefault();
                if (shop is null)
                {
                    throw new ArgumentException("Shop not Found!");
                }
                var p = (from x in context.Products where x.ProductType.ShopId == Id select x).FirstOrDefault();
                if (p is null)
                {
                    throw new ArgumentException("Product not Found!");
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = (from pd in context.Products
                            join pt in context.ProductTypes
                            on pd.TypeId equals pt.TypeId
                            where pt.ShopId == Id && pd.Status == "ON"
                            select new
                            {
                                ProductId = p.ProductId,
                                ProductImage = p.Image,
                                ProductName = p.ProductName,
                                Price = p.Price,
                                ProductType = p.ProductType,
                                Description = p.Description,
                            }).ToList(),
                };
            }
        }
        public Result AddOrder(Model.Employee.Order Value, int UserId, int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var Total = 0;
                    var emp = (from obj in context.EmployeeDetails
                               where obj.UserId == UserId
                               select obj).SingleOrDefault();

                    if (emp is null)
                    {
                        throw new ArgumentException("Employee Not Found!");
                    }

                    var p = (from x in context.Products where x.ProductType.ShopId == Id select x).FirstOrDefault();
                    if (p is null)
                    {
                        throw new ArgumentException("Product is not from this shop");
                    }




                    var OM = (from x in context.OrderMstrs where x.Status == "Not Requested" && x.EmployeeId == emp.EmployeeId select x).FirstOrDefault();

                    if (OM is null)
                    {
                        OM = new OrderMstr()
                        {
                            ShopId = Id,
                            OfficeId = emp.OfficeId,
                            EmployeeId = emp.EmployeeId,
                            Status = "Not Requested",
                            Total = Total,
                        };


                        context.OrderMstrs.InsertOnSubmit(OM);
                        context.SubmitChanges();

                    }
                    else
                    {
                        if (OM.ShopId != Id)
                        {
                            throw new ArgumentException("Can not add Items of multiple shops");
                        }
                    }

                    var OD = new OrderDetail()
                    {
                        OrderId = OM.OrderId,
                        ProductId = Value.Product.Id,
                        Description = Value.Description,

                    };

                    context.OrderDetails.InsertOnSubmit(OD);
                    context.SubmitChanges();
                    // scope.Complete();

                    var qs = (from obj in context.OrderDetails
                              where obj.OrderId == OM.OrderId
                              select obj).ToList();
                    foreach (var item in qs)
                    {
                        var price = (from obj in context.Products
                                     where obj.ProductId == item.ProductId
                                     select obj.Price).SingleOrDefault();
                        Total = Total + (int)price;
                    }
                    var q = (from obj in context.OrderMstrs
                             where obj.OrderId == OM.OrderId
                             select obj).SingleOrDefault();

                    q.Total = Total;
                    context.SubmitChanges();
                    scope.Complete();

                    return new Result()
                    {
                        Message = string.Format($"Order Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = OM.OrderId,
                    };
                }
            }
        }
        public Result GetCart(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var emp = (from obj in context.EmployeeDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                return new Result()
                {
                    Message = string.Format($"Get Order Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = (from OM in context.OrderMstrs
                            join OD in context.OrderDetails
                            on OM.OrderId equals OD.OrderId
                            join PT in context.Products
                            on OD.ProductId equals PT.ProductId
                            where OM.Status == "Not Requested" && OM.EmployeeId == emp.EmployeeId
                            select new
                            {
                                ProductId = OD.ProductId,
                                Image = PT.Image,
                                ProductName = PT.ProductName,
                                Price = PT.Price,
                                Total = OM.Total,
                            }).ToList(),
                };
            }
        }
        public Result UpdateStatus(int UserId,int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                OrderMstr orderMstr = context.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
                if(orderMstr is not null)
                {
                    if (orderMstr.Status == "Not Requested")
                    {
                        orderMstr.Status = "Requested";
                        context.SubmitChanges();
                        return new Result()
                        {
                            Message = string.Format($" Order Requested to Manager"),
                            Status = Result.ResultStatus.success,

                        };
                    }
                    return new Result()
                    {
                            Message = string.Format($" Order Already Requested to Manager"),
                            Status = Result.ResultStatus.success,
                    };

                }
                return new Result()
                {
                    Message = string.Format($" Order Id Not Found"),
                    Status = Result.ResultStatus.danger,
                };
            }
        }
    }
}
