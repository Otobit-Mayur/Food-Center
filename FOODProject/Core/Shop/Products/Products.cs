using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Shop.Products
{
    public class Products
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result AddProuct(Model.Shop.Product.Product value)
        {
            FoodCenterContext.Product p = new Product();

            var res = context.ProductTypes.SingleOrDefault(x => x.Type == value.Type.String);
            /* var ft = context.FixLookUps.FirstOrDefault(x => x.FixName == value.FoodType.Id);*/
            //.EmailId == value.UserId.String
           /* var ft = from od in context.FixLookUps
                     where od.FixId == value.FoodType.Id
                     select od.FixName;*/
            p.ProductName = char.ToUpper(value.ProductName[0]) + value.ProductName.Substring(1).ToLower();
            
            var check = context.Products.FirstOrDefault(x => x.ProductName == value.ProductName);
            if (check != null)
            {
                return new Result()
                {
                    Message = string.Format("Product Already Exits"),
                    Status = Result.ResultStatus.danger,
                };
            }
            else
            {
                p.Price = value.Price;
                p.Description = value.Description;
                p.Image = value.Image;
                p.TypeId = res.TypeId;
                p.Status = "ON";
               /* p.FoodType = ft.ToString();*/
                context.Products.InsertOnSubmit(p);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"New Product Added Successfully"),
                    Status = Result.ResultStatus.success,
                };
            }

        }
        public Result GetAllProduct(int UserId)
        {
            var qes = (from obj in context.ShopDetails
                       where obj.UserId == UserId
                       select obj.ShopId).SingleOrDefault();
            return new Result()
            {
                Message = string.Format("Get All Product Successfully"),
                Status = Result.ResultStatus.none,
                Data = (from obj in context.Products
                        join pt in context.ProductTypes
                        on obj.TypeId equals pt.TypeId
                        where obj.ProductType.ShopId==qes && obj.Status != "DELETE"
                        select new
                        {
                            ProductId = obj.ProductId,
                            ProductName = obj.ProductName,
                            Price = obj.Price,
                            Description = obj.Description,
                            Image = obj.Image,
                            TypeId = obj.TypeId,
                            Type = pt.Type,
                            ShopId = pt.ShopId
                        }).ToList(),
            };
        }
        public Result Update(Model.Shop.Product.Product value, int Id)
        {
            

            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                product.ProductName = char.ToUpper(value.ProductName[0]) + value.ProductName.Substring(1).ToLower();
                var check = context.Products.FirstOrDefault(x => x.ProductName == value.ProductName);
                product.Price = value.Price;
                product.Description = value.Description;
                product.Image = value.Image;
                product.TypeId = value.Type.Id;

                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Product Updated"),
                    Status = Result.ResultStatus.success,
                };
            }
            return new Result()
            {
                Message = string.Format($"Product Not Available"),
                Status = Result.ResultStatus.warning,
            };
        }

        public Result UpdateStatus(int Id)
        {
            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                if (product.Status == "ON")
                {
                    product.Status = "OFF";
                }
                else
                {
                    product.Status = "ON";
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

        public Result Delete(int Id)
        {
            //FoodCenterContext.Product p = new Product();

            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                product.Status = "DELETE";
                //context.Products.DeleteOnSubmit(product);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Product Deleted"),
                    Status = Result.ResultStatus.success,
                };
            }
            return new Result()
            {
                Message = string.Format($"Product Not Available"),
                Status = Result.ResultStatus.warning,
            };
        }
        

        // Filtering in Product
        public Result GetByType(int TypeId)
        {
            return new Result()
            {
                Message = string.Format($"Get Product Successfully"),
                Status = Result.ResultStatus.success,
                Data = (from obj in context.Products
                        join pt in context.ProductTypes
                        on obj.TypeId equals pt.TypeId
                        where obj.TypeId == TypeId
                        select new
                        {
                            ProductId = obj.ProductId,
                            ProductName = obj.ProductName,
                            Price = obj.Price,
                            Description = obj.Description,
                            Image = obj.Image,
                            TypeId = obj.TypeId,
                            Type = pt.Type,
                            ShopId = pt.ShopId
                        }).ToList(),
            };

            
        }

        //Sort By Price In Ascending order 
        public Result SortByPrice ()
        {
            return new Result()
            {
                Message = string.Format("Get Product In Sort By Price Successfully"),
                Status = Result.ResultStatus.none,
                Data =(from obj in context.Products
                                join pt in context.ProductTypes
                                on obj.TypeId equals pt.TypeId
                                orderby obj.Price

                                select new
                                {
                                    ProductId = obj.ProductId,
                                    ProductName = obj.ProductName,
                                    Price = obj.Price,
                                    Description = obj.Description,
                                    Image = obj.Image,
                                    TypeId = obj.TypeId,
                                    Type = pt.Type,
                                    ShopId = pt.ShopId
                                }).ToList(),
            };
          
        }
        //Sort By Price In Descending order 
        public Result SortByPriceDes()
        {
            return new Result()
            {
                Message = string.Format("Get Product In Sort By Price in Descending Order"),
                Status = Result.ResultStatus.none,
                Data = (from obj in context.Products
                        join pt in context.ProductTypes
                        on obj.TypeId equals pt.TypeId
                        orderby obj.Price descending
                        select new
                        {
                            ProductId = obj.ProductId,
                            ProductName = obj.ProductName,
                            Price = obj.Price,
                            Description = obj.Description,
                            Image = obj.Image,
                            TypeId = obj.TypeId,
                            Type = pt.Type,
                            ShopId = pt.ShopId
                        }).ToList(),

            };
        }
    }
}
