using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Common
{
    public class Orders
    {

        FoodCenterDataContext db = new FoodCenterDataContext();
        OrderDetail od = new OrderDetail();
        OrderMstr om = new OrderMstr();

        public Result UpdateStatus(int Id, Model.Common.Order value)
        {
            OrderMstr om = db.OrderMstrs.SingleOrDefault(x => x.OrderId == Id);
            if (om != null)
            {
                if (om.Track == null)
                {
                    var t = value.Time.Id * 10;
                    om.Status = 1;
                    om.Time = DateTime.Now.AddMinutes(t);//value.Time.String,
                    om.Track = 0;
                    om.DateTime = DateTime.Now;
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
            OrderMstr om = db.OrderMstrs.SingleOrDefault(x => x.OrderId == Id);
            if (om != null)
            {
                if (om.Status == 1)
                {
                    om.Track = 1;
                    om.DateTime = DateTime.Now;
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
        public Result GetAll(int UserId)
        {

            var sid = (from shop in db.ShopDetails
                       where shop.UserId == UserId
                       select shop.ShopId).SingleOrDefault();
            //return sid;
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
                        where oom.ShopId == sid
                        select new
                        {
                            Image = prod.Image,
                            ItemName = prod.ProductName,
                            Qty = od.Qty,
                            OfficeName = o.Name,
                            OfficeAddress = oa.Address
                        }).ToList(),
            };

        }
        public Result GetId()
        {
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format("Get All Detail Successfully"),
                Data = (from l in db.LookUps
                        join fl in db.FixLookUps
                        on l.LookUpId equals fl.LookUpId
                        where fl.LookUpId == 1
                        select new
                        {
                            TimeId = fl.FixId,
                            Time = fl.FixName
                        }).ToList(),

            };
        }
    }
}


