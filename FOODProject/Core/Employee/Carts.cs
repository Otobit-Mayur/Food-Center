using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Employee
{
    public class Carts
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result GetCartOrder(int UserId)
        {
            var emp = (from obj in context.EmployeeDetails
                          where obj.UserId == UserId
                          select obj).SingleOrDefault();
            if (emp is null)
            {
                throw new ArgumentException("Employee not found!");
            }
            if (emp != null)
            {
                var count = (from om in context.OrderDetails
                             where om.OrderMstr.EmployeeId == emp.EmployeeId && om.OrderMstr.Status == "Not Requested" 
                             select om).Count();
                if (count != 0)
                {
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Get All Order Request Successfully"),
                        Data = (from om in context.OrderMstrs
                                join od in context.OrderDetails
                                on om.OrderId equals od.OrderId
                                join p in context.Products
                                on od.ProductId equals p.ProductId
                                join e in context.EmployeeDetails
                                on om.EmployeeId equals e.EmployeeId
                                where om.EmployeeId == emp.EmployeeId && e.EmployeeId == emp.EmployeeId && om.Status == "Not Requested"
                                select new
                                {
                                    ShopName = om.ShopDetail.ShopName,
                                    ProductName = p.ProductName,
                                    Price = p.Price,
                                    ProductImage = p.Image,
                                    OrderDetailId = od.OrderDetailId,
                                    OrderId = om.OrderId,
                                    TotalItem = count,
                                    Total = om.Total,
                                }
                                ).ToList(),
                    };
                }
                else
                {
                    return new Result()
                    {
                        Message = string.Format($"You Dont Have Any Order To Request"),
                        Status = Result.ResultStatus.success,
                        Data = emp.EmployeeId,
                    };
                }

            }
            else
            {
                throw new ArgumentException("Some Unknown Error Is Occure");
            }
        }
        public Result DeleteOrder(int Id,int UserId)
        {
            var emp = (from obj in context.EmployeeDetails
                       where obj.UserId == UserId
                       select obj).SingleOrDefault();
            if (emp is null)
            {
                throw new ArgumentException("Employee not found!");
            }
            OrderDetail od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == Id);
            if (od is null)
            {
                throw new ArgumentException("This Order Detail is not available  ");
            }
            OrderMstr om = context.OrderMstrs.FirstOrDefault(x => x.OrderId == od.OrderId);
            om.Total = om.Total - od.Product.Price;
            context.SubmitChanges();
            context.OrderDetails.DeleteOnSubmit(od);
            context.SubmitChanges();

            return new Result()
            {
                Message = string.Format($"Deleted Successfully"),
                Status = Result.ResultStatus.success,
                Data = od.OrderDetailId,
            };
        }
        public Result OrderRequest(int UserId)
        {
            var emp = (from obj in context.EmployeeDetails
                       where obj.UserId == UserId
                       select obj).SingleOrDefault();
            if (emp is null)
            {
                throw new ArgumentException("Employee not found!");
            }
            OrderMstr om = context.OrderMstrs.Where(x=>x.Status == "Not Requested").FirstOrDefault(x => x.EmployeeId == emp.EmployeeId);
            if (om is null)
            {
                throw new ArgumentException("This Employee Dont Have Any Order To Request ");
            }
           
            if (om.Status == "Not Requested")
            {
                om.Status = "Requested";
                context.SubmitChanges();

                return new Result()
                {
                    Message = string.Format($"Order Requested Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = om.OrderId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error is Occure");
            }
        }
        public Result GetCartHistory(int UserId)
        {
            var emp = (from obj in context.EmployeeDetails
                       where obj.UserId == UserId
                       select obj).SingleOrDefault();
            if (emp is null)
            {
                throw new ArgumentException("Employee not found!");
            }
            if (emp != null)
            {
                var count = (from om in context.OrderMstrs
                             where om.EmployeeId == emp.EmployeeId && om.Status != "Not Requested"
                             select om).Count();
                if (count != 0)
                {
                      
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Get Order History Successfully"),
                        Data = (from om in context.OrderMstrs
                                join od in context.OrderDetails
                                on om.OrderId equals od.OrderId
                                join p in context.Products
                                on od.ProductId equals p.ProductId
                                join e in context.EmployeeDetails
                                on om.EmployeeId equals e.EmployeeId
                                where om.EmployeeId == emp.EmployeeId && e.EmployeeId == emp.EmployeeId && om.Status != "Not Requested"
                                select new
                                {
                                    ShopName = om.ShopDetail.ShopName,
                                    ProductName = p.ProductName,
                                    Price = p.Price,
                                    ProductImage = p.Image,
                                    OrderDetailId = od.OrderDetailId,
                                    OrderId = om.OrderId,
                                    Status = om.Status,
                                }
                                ).ToList(),
                    };
                }
                else
                {
                    return new Result()
                    {
                        Message = string.Format($"You Dont Have An Order History"),
                        Status = Result.ResultStatus.success,
                        Data = emp.EmployeeId,
                    };
                }

            }
            else
            {
                throw new ArgumentException("Some Unknown Error Is Occure");
            }
        }
    }
}
