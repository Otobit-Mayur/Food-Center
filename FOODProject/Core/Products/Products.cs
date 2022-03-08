using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Core.Products
{
    public class Products
    {
        FoodCenterDataContext context = new FoodCenterDataContext();

        public Result AddProuct(Model.Product.Product value)
        {
            FoodCenterContext.Product p = new Product();

            var res = context.ProductTypes.SingleOrDefault(x => x.Type == value.TypeId.String);

            p.ProductName = value.ProductName;
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
                p.Status = 1;
                context.Products.InsertOnSubmit(p);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"New Product Added Successfully"),
                    Status = Result.ResultStatus.success,
                };
            }

        }
        public async Task<IEnumerable>get()
        {
            /*var q = (from obj in context.Products
                     select new { obj.ProductId, obj.ProductName,obj.Price, obj.Description, obj.Status, obj.Image }).ToList();*/
            var q = (from obj in context.Products
                     select obj).ToList();

            return q;
        }
        public Result Update(Model.Product.Product value, int Id)
        {
            

            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                product.ProductName = value.ProductName;
                product.Price = value.Price;
                product.Description = value.Description;
                product.Image = value.Image;
                product.TypeId = value.TypeId.Id;

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
                if (product.Status == 1)
                {
                    product.Status = 0;
                }
                else
                {
                    product.Status = 1;
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

        public Result Delete(Model.Product.Product value, int Id)
        {
            //FoodCenterContext.Product p = new Product();

            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                context.Products.DeleteOnSubmit(product);
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
    }
}
