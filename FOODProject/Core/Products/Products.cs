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

            p.ProductName = char.ToUpper(value.ProductName[0])+value.ProductName.Substring(1).ToLower();
            //char.ToUpper(userModel.UserName[0]) + userModel.UserName.Substring(1).ToLower();
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
                     join pt in context.ProductTypes
                     on obj.TypeId equals pt.TypeId
                     select new
                     {
                         ProductId=obj.ProductId,
                         ProductName=obj.ProductName,
                         Price=obj.Price,
                         Description=obj.Description,
                         Image=obj.Image,
                         TypeId=obj.TypeId,
                         Type=pt.Type,
                         ShopId=pt.ShopId
                     }).ToList();

            return q;
        }
        public Result Update(Model.Product.Product value, int Id)
        {
            

            Product product = context.Products.SingleOrDefault(x => x.ProductId == Id);
            if (product != null)
            {
                product.ProductName = char.ToUpper(value.ProductName[0]) + value.ProductName.Substring(1).ToLower();
                var check = context.Products.FirstOrDefault(x => x.ProductName == value.ProductName);
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
        public Result getsid(string Emailid)
        {
            var uid = (from user in context.Users
                       where user.EmailId == Emailid
                       select user.UserId).SingleOrDefault();
            return new Result()
            {
                Message = string.Format($"Product Not Available"),
                Status = Result.ResultStatus.warning,
                Data=uid,
            };
        }

        // Filtering in Product
        public async Task<IEnumerable> GetByType(int TypeId)
        {
         
            var q = (from obj in context.Products
                     join pt in context.ProductTypes
                     on obj.TypeId equals pt.TypeId
                     where obj.TypeId==TypeId
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
                     }).ToList();

            return q;
        }

        //Sort By Price In Ascending order 
        public async Task<IEnumerable> SortByPrice ()
        {

            var q = (from obj in context.Products
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
                     }).ToList();

            return q;
        }
        //Sort By Price In Descending order 
        public async Task<IEnumerable> SortByPriceDes()
        {

            var q = (from obj in context.Products
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
                     }).ToList();

            return q;
        }
    }
}
