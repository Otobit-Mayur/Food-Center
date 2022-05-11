using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop
{
    public class ShopHomes
    {
        public Result GetShopDetail(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if(qes==null)
                {
                    throw new ArgumentException("Shop Id not Found");
                }
                return new Result()
                {
                    Message = string.Format($"Get Detail Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = (from image in context.Images
                            join obj in context.ShopDetails
                            on image.ImageId equals obj.Logo
                            join add in context.ShopAddresses
                            on obj.ShopId equals add.ShopId
                            where obj.ShopId==qes.ShopId
                            select new
                            {
                                Logo=obj.Logo,
                                ShopName=obj.ShopName,
                                Address=add.AddressLine,
                                Status=obj.Status,
                                Path=image.Path
                            }).ToList(),
                 };
            }
        }
        public Result UpdateStatus(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if (qes == null)
                {
                    throw new ArgumentException("Shop Id not Found");
                }
                ShopDetail SD = context.ShopDetails.SingleOrDefault(x => x.ShopId == qes.ShopId);
                if (SD != null)
                {
                    if (SD.Status == "ON")
                    {
                        SD.Status = "OFF";
                    }
                    else
                    {
                        SD.Status = "ON";
                    }
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Status Updated"),
                        Status = Result.ResultStatus.success,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"Product Not Available"),
                    Status = Result.ResultStatus.danger,
                };
            }
        }
        public Result GetProductType(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj).FirstOrDefault();
                if (qes == null)
                {
                    throw new ArgumentException("Shop Id not Found");
                }
                return new Result()
                {
                    Message = string.Format("Get All Product Type Sccessfully"),
                    Status = Result.ResultStatus.warning,
                    Data = (from obj in context.ProductTypes
                            where obj.ShopId==qes.ShopId
                            select new 
                            {
                               TypeId=obj.TypeId,
                               Type=obj.Type 
                            }).ToList(),
                };
            }
        }
        public Result TodaysOrder(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var shop = (from sp in context.ShopDetails
                            where sp.UserId == UserId
                            select sp).FirstOrDefault();
                if (shop is null)
                {
                    throw new ArgumentException("Shop not found!");
                }
                var qs = (from oom in context.OrderMstrs
                          join od in context.OrderDetails
                          on oom.OrderId equals od.OrderId
                          where Convert.ToDateTime(od.OrderTime).Date == DateTime.Now.Date && oom.ShopId == shop.ShopId && oom.Status == "Approved"
                          select oom).Count();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = qs,
                };
            }
        }
        public Result GetTopOrder(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var shop = (from sp in context.ShopDetails
                            where sp.UserId == UserId
                            select sp).FirstOrDefault();
                if (shop is null)
                {
                    throw new ArgumentException("Shop not found!");
                }
                var products = (from order in context.OrderDetails
                                join om in context.OrderMstrs
                                on order.OrderId equals om.OrderId
                                join p in context.Products
                                on order.ProductId equals p.ProductId
                                where om.ShopId == shop.ShopId
                                group order.Qty by order.ProductId into ans
                                orderby ans.Sum() descending
                                select new
                                {
                                    ProductId = ans.Key,
                                    Order = ans.Sum(),
                                    ProductName = context.Products.Where(x => x.ProductId == ans.Key).Select(x => x.ProductName),
                                    ProductImage = context.Products.Where(x => x.ProductId == ans.Key).Select(x => x.Image),
                                }).Take(5);
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Get All Detail Successfully"),
                    Data = products,
                };

            }

        }
    }
}
