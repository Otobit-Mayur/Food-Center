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

            var res = context.ProductTypes.SingleOrDefault(x => x.TypeId == value.Type.Id);
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

                var I = context.Images.FirstOrDefault(i => i.ImageId == value.Image.Id);
                if (I == null)
                {
                    throw new ArgumentException("Invalid Image Id");
                }
                I.IsDeleted = false;
                context.SubmitChanges();


                p.ImageId = I.ImageId;
                p.TypeId = res.TypeId;
                p.FoodType = value.FoodType.Id;
                p.Status = true;
                p.IsDeleted = false;
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
        public Result GetFoodType()
        {
            return new Result()
            {
                Message = string.Format("Get All Product Successfully"),
                Status = Result.ResultStatus.none,
                Data = (from obj in context.FixLookUps
                        where obj.LookUpId==6
                        select new Model.Common.IntegerNullString()
                        {
                            Id = obj.FixId,
                            Text = obj.FixName
                        }).ToList(),
            };
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
                        where obj.ProductType.ShopId==qes && obj.IsDeleted==false
                        select new
                        {
                            ProductId = obj.ProductId,
                            ProductName = obj.ProductName,
                            Price = obj.Price,
                            Description = obj.Description,
                            Image = obj.ImageId,
                            Path=obj.ImageId == 0? null: obj.Image.Path,
                            TypeId = obj.TypeId,
                            Type = obj.ProductType.Type,
                            ShopId = obj.ProductType.ShopId,
                            Status=obj.Status,
                            FoodType=obj.FixLookUp.FixName
                        }).ToList(),
            };
        }

        public Result GetById(int Id)
        {
            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if(product==null)
            {
                throw new ArgumentException("Invalid Id");
            }
            return new Result()
            {
                Message = string.Format("Get Product Successfully"),
                Status = Result.ResultStatus.success,
                Data = (from p in context.Products
                        where p.ProductId == Id
                        select new
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            Price = p.Price,
                            Description = p.Description,
                            Image = p.ImageId,
                            Path = p.ImageId == 0 ? null : p.Image.Path,
                            TypeId = p.TypeId,
                            Type = p.ProductType.Type,
                            ShopId = p.ProductType.ShopId,
                            Status = p.Status,
                            FoodType = p.FixLookUp.FixName
                        }).FirstOrDefault(),
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
                product.FoodType = value.FoodType.Id;

                var I = context.Images.FirstOrDefault(i => i.ImageId == value.Image.Id);
                if (I == null)
                {
                    throw new ArgumentException("Invalid Image Id");
                }

                I.IsDeleted = false;
                context.SubmitChanges();


                product.ImageId = I.ImageId;
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
                if (product.Status == true)
                {
                    product.Status = false;
                }
                else
                {
                    product.Status = true;
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
                Message = string.Format($"Product ID Not Available"),
                Status = Result.ResultStatus.danger,
            };
        }

        public Result Delete(int Id)
        {
            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                product.IsDeleted=true;
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
                        where obj.TypeId == TypeId
                        select new
                        {
                            ProductId = obj.ProductId,
                            ProductName = obj.ProductName,
                            Price = obj.Price,
                            Description = obj.Description,
                            Image = obj.ImageId,
                            TypeId = obj.TypeId,
                            Type = obj.ProductType.Type,
                            ShopId = obj.ProductType.ShopId,
                            FoodType = obj.FixLookUp.FixName
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
