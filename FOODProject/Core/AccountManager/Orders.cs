using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.AccountManager
{
    public class Orders
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result GetAllOrderRequest(int UserId)
        {
            var office = (from obj in context.OfficeDetails
                          where obj.UserId == UserId
                          select obj).SingleOrDefault();
            if (office is null)
            {
                throw new ArgumentException("Office not found!");
            }
            if (office != null)
            {
                var count = (from om in context.OrderMstrs
                             where om.OfficeId == office.OfficeId && om.Status == "Requested"
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
                                where om.OfficeId == office.OfficeId && e.OfficeId == office.OfficeId && om.Status == "Requested" && od.IsRejected == "False"
                                select new
                                {
                                    EmployeeId = e.EmployeeId,
                                    EmployeeName = e.EmployeeName,
                                    EmployeePhoto = e.Photo,
                                    OfficeLocaqtion = e.OfficeLocation,
                                    ShopName = om.ShopDetail.ShopName,
                                    ProductName = p.ProductName,
                                    Price = p.Price,
                                    ProductImage = p.Image,
                                    OrderDetailId = od.OrderDetailId,
                                    OrderId = om.OrderId,
                                }
                                ).ToList(),
                    };
                }
                else
                {
                    return new Result()
                    {
                        Message = string.Format($"You Dont Have Any Order Request"),
                        Status = Result.ResultStatus.success,
                        Data = office.OfficeId,
                    };
                }

            }
            else
            {
                throw new ArgumentException("Some Unknown Error Is Occure");
            }
        }
        public Result DeleteOrder(int Id)
        {
            OrderDetail od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == Id);
            if (od is null)
            {
                throw new ArgumentException("This Order Detail is not available  ");
            }
            OrderMstr om = context.OrderMstrs.FirstOrDefault(x => x.OrderId == od.OrderId);
            if (om.Status == "Approved")
            {
                throw new ArgumentException("Once you Approve the order than you can not Change that  ");
            }
           
            if (od.IsRejected == "True")
            {
                throw new ArgumentException("Once you Delete the order than you can not Accept that  ");
            }
          
            if (od.IsRejected == "False")
            {
                od.IsRejected = "True";
                context.SubmitChanges();
                var c = (from o in context.OrderDetails
                         where o.OrderId == od.OrderId && o.IsRejected == "False" 
                         select o).SingleOrDefault();
                if(c is null)
                {
                    om.Status = "Not Approved";
                    context.SubmitChanges();
                }
                return new Result()
                {
                    Message = string.Format($"Status Updated Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = od.OrderId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error is Occure");
            }
        }
        public Result ApproveOrder(int Id)
        {
            OrderMstr om = context.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
            if (om is null)
            {
                throw new ArgumentException("This Order  is not available  ");
            }
            if (om.Status == "Not Approved")
            {
                throw new ArgumentException("Once you Reject the order than you can not Approve that  ");
            }
            if (om.Status == "Approved")
            {
                throw new ArgumentException("Already Approved");
            }
            if (om.Status == "Requested")
            {
                om.Status = "Approved";
                context.SubmitChanges();
       
                return new Result()
                {
                    Message = string.Format($"Order Approved Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = om.OrderId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error is Occure");
            }
        }
        public Result RejectOrder(int Id)
        {
            OrderMstr om = context.OrderMstrs.FirstOrDefault(x => x.OrderId == Id);
            if (om is null)
            {
                throw new ArgumentException("This Order  is not available  ");
            }
            if (om.Status == "Approved")
            {
                throw new ArgumentException("Once you Approved the order than you can not Reject that  ");
            }
            if (om.Status == "Not Approved")
            {
                throw new ArgumentException("Already Rejected");
            }
            if (om.Status == "Requested")
            {
                om.Status = "Not Approved";
                context.SubmitChanges();

                return new Result()
                {
                    Message = string.Format($"Order Rejected Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = om.OrderId,
                };
            }
            else
            {
                throw new ArgumentException("Some Unknown Error is Occure");
            }
        }
    }
}
