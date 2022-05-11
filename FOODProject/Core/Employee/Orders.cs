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
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result AddOrder(Model.Employee.Order Value, int UserId, int Id)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var emp = (from obj in context.EmployeeDetails
                           where obj.UserId == UserId
                           select obj).SingleOrDefault();
                if (emp is null)
                {
                    throw new ArgumentException("Employee Not Found!");
                }
                var e1 = (from obj in context.EmployeeDetails
                          where obj.EmployeeId == emp.EmployeeId && (obj.IsDeleted == true || obj.IsActive == false)
                          select obj).SingleOrDefault();
                if (e1 != null)
                {
                    throw new ArgumentException("Maybe This Employee is On Leave Or Maybe Deleted Permentely!");
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
                        Total = 0,

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
                    //Description = Value.Description,
                    IsRejected = "False",
                    Qty = 1,

                };
                Product prod = context.Products.Where(x => x.ProductId == OD.ProductId).FirstOrDefault();
                OM.Total = OM.Total + prod.Price;
                context.OrderDetails.InsertOnSubmit(OD);
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


        public Result GetAllSubscriber(int UserId)
        {
            var emp = (from obj in context.EmployeeDetails
                       where obj.UserId == UserId
                       select obj).SingleOrDefault();
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
                Data = (from s in context.Subscribers
                        join sd in context.ShopDetails
                        on s.ShopId equals sd.ShopId
                        join sa in context.ShopAddresses
                        on sd.ShopId equals sa.ShopId
                        where s.Subscription == "Accepted"  && s.Status==true && s.OfficeId==emp.OfficeId
                        select new
                        {
                            ShopId=sd.ShopId,
                            ShopName = sd.ShopName,
                            Description=sd.Description,
                            ShopAddress = sa.AddressLine,
                            Logo = sd.Logo,
                            LogoPath = sd.Logo == 0 ? null : sd.Image_Logo.Path,
                            Distance = GetDistanceFromShop(p1, sa.AddressId)
                        }).ToList(),
            };
        }
        public double GetDistanceFromShop(OfficeAddress p1, int Id)
        {
            var p2 = (from x in context.ShopAddresses where x.AddressId == Id select x).FirstOrDefault();

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
        public Result GetAllProduct(int Id)
        {
            var shop = (from x in context.ShopDetails where x.ShopId == Id  select x).FirstOrDefault();
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
                        where pd.ProductType.ShopId == Id && pd.Status==true 
                        select new
                        {
                           ProductId = pd.ProductId,
                           ProductImage = pd.ImageId,
                           Path=pd.ImageId==0?null:pd.Image.Path,
                           ProductName = pd.ProductName,
                           Price = pd.Price,
                           ProductType = pd.ProductType.Type,
                           Description = pd.Description,
                           ShopId=pd.ProductType.ShopId
                         //ShopDescription=pd.ProductType.ShopDetail.Description
                        }).ToList(),
            };
        }
    }
}
