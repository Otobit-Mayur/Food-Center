using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Core.Shop.StoreDetails
{
    public class StoreDetails
    {
        public Result AddStoreDetails(Model.Shop.ShopDetail.ShopDetail value)
        {

            using (TransactionScope scope = new TransactionScope())
            {
                using (FoodCenterDataContext context = new FoodCenterDataContext())
                {
                    ShopDetail sd = new ShopDetail();
                    sd.ShopName = value.ShopName;
                    sd.PhoneNumber = value.PhoneNumber;
                    sd.AlternateNumber = value.AlternateNumber;
                    sd.DeliveryRadius = value.DeliveryRadius;
                    sd.Status = "ON";

                    //Insert Image Id
                    var I = context.Images.FirstOrDefault(i => i.ImageId == value.Logo.Id);
                    if (I == null)
                    {
                        throw new ArgumentException("Invalid Image Id");
                    }
                    I.IsDeleted = false;
                    context.SubmitChanges();
                    sd.Logo = I.ImageId;


                    var ci = context.Categories.SingleOrDefault(c => c.CategoryId == value.Category.Id);

                    var ui = context.Users.SingleOrDefault(c => c.UserId == value.User.Id);
                    var check = context.ShopDetails.SingleOrDefault(c => c.UserId == ui.UserId);
                    if (check != null)
                    {
                        throw new ArgumentException("Entered Email Already Registered for another store");
                    }

                    sd.CategoryId = ci.CategoryId;
                    sd.UserId = ui.UserId;

                    context.ShopDetails.InsertOnSubmit(sd);
                    context.SubmitChanges();
                    
                    FoodCenterContext.ShopAddress add = new FoodCenterContext.ShopAddress()
                    {
                        AddressLine = value.Address.AddresssLine,
                        Latitude = value.Address.Latitude,
                        Longitude = value.Address.Longitude,
                        ShopId = sd.ShopId,
                    };
                    context.ShopAddresses.InsertOnSubmit(add);
                    context.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {

                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = sd.ShopId,
                    };
                }          
            }
            
        }
        public Result GetCurrentShop(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.ShopDetails
                           where obj.UserId == UserId
                           select obj.ShopId).SingleOrDefault();
                if (qes != null)
                {
                    return new Result()
                    {
                        Message = String.Format($"Get Current Shop Details"),
                        Status = Result.ResultStatus.success,
                        Data = (from obj in context.ShopDetails
                                join add in context.ShopAddresses
                                on obj.ShopId equals add.ShopId
                                where obj.ShopId==qes
                                select new
                                {
                                    ShopId=obj.ShopId,
                                    ShopName=obj.ShopName,
                                    PhoneNo=obj.PhoneNumber,
                                    AlternateNo=obj.AlternateNumber,
                                    DeliveryRadius=obj.DeliveryRadius, 
                                    Status = obj.Status,
                                    Description=obj.Description,
                                    Logo=obj.Logo,
                                    LogoPath=obj.Logo==0?null:obj.Image_Logo.Path,
                                    CoverPhoto=obj.CoverPhoto,
                                    CoverPath=obj.CoverPhoto==0?null: obj.Image_CategoryId.Path,
                                    UserId=obj.UserId,
                                    EmailId=obj.User.EmailId,
                                    CategoryId = obj.CategoryId,
                                    CategoryName= obj.Category.CategoryName,
                                    AddressId=add.AddressId,
                                    AddressLine=add.AddressLine
                                }).FirstOrDefault(),

                    };
                }
                else
                {
                    return new Result()
                    {
                        Message = String.Format($"Invalid Current Shop "),
                        Status = Result.ResultStatus.success,
                    };
                }
            }
              
        }

        public Result UpdateProfile(Model.Shop.ShopDetail.UpdateShopDetails value,int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    var qes = (from obj in context.ShopDetails
                               where obj.UserId == UserId
                               select obj.ShopId).SingleOrDefault();
                    ShopDetail shopdetail = context.ShopDetails.SingleOrDefault(x => x.ShopId == qes);
                    if (shopdetail != null)
                    {
                        shopdetail.ShopName = value.ShopName;
                        shopdetail.PhoneNumber = value.PhoneNumber;
                        //shopdetail.Logo = value.Logo;
                        shopdetail.Description = value.Description;
                        context.SubmitChanges();

                        var I = context.Images.FirstOrDefault(i => i.ImageId == value.Logo.Id);
                        if (I != null)
                        {
                            shopdetail.Logo = I.ImageId;
                            I.IsDeleted = false;
                            context.SubmitChanges();
                           
                        }

                        var C = context.Images.FirstOrDefault(c => c.ImageId == value.CoverPhoto.Id);
                        if (C != null)
                        {
                            shopdetail.CoverPhoto = C.ImageId;
                            C.IsDeleted = false;
                            context.SubmitChanges();
                        }

                        context.SubmitChanges();

                        
                        ShopAddress sa = context.ShopAddresses.FirstOrDefault(x => x.ShopId ==shopdetail.ShopId);
                        if(sa!=null)
                        {
                            sa.AddressLine = value.Address.AddresssLine;
                            sa.Latitude = value.Address.Latitude;
                            sa.Longitude = value.Address.Longitude;
                            context.SubmitChanges();
                        }
                        else
                        {
                            return new Result()
                            {

                                Message = string.Format($"Address Is Not Valid"),
                                Status = Result.ResultStatus.danger,
                            };
                        }
                        scope.Complete();
                        return new Result()
                        {

                            Message = string.Format($"Profile Updated Successfully"),
                            Status = Result.ResultStatus.success,
                            Data=shopdetail.ShopId
                        };
                    }
                    else
                    {
                        return new Result()
                        {

                            Message = string.Format($"Invalid Shop ID"),
                            Status = Result.ResultStatus.danger,
                        };
                    }
                }
            }
                
        }
           
    }
}

