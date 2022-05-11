using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Core.AccountManager.OfficeDetails
{
    public class OfficeDetails
    {
        public Result AddOfficeDetail(Model.AccountManager.OfficeDetail.OfficeDetail value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (FoodCenterDataContext context = new FoodCenterDataContext())
                {
                    OfficeDetail OD = new OfficeDetail();

                    OD.OfficeName = value.OfficeName;
                    OD.ManagerName = value.Managername;
                    OD.PhoneNumber = value.PhoneNumber;
                    OD.AlternateNumber = value.AlternateNumber;

                    var I = context.Images.FirstOrDefault(i => i.ImageId == value.Image.Id);
                    if (I == null)
                    {
                        throw new ArgumentException("Invalid Image Id");
                    }
                    I.IsDeleted = false;
                    context.SubmitChanges();
                    OD.Image = I.ImageId;

                    // OD.Image = value.Image;

                    var ui = context.Users.SingleOrDefault(c => c.UserId == value.User.Id);
                    var check = context.ShopDetails.SingleOrDefault(c => c.UserId == ui.UserId);
                    if (check != null)
                    {
                        throw new ArgumentException("Entered Email Already Registered for another store");
                    }

                    OD.UserId = ui.UserId;
                    context.OfficeDetails.InsertOnSubmit(OD);
                    context.SubmitChanges();

                    FoodCenterContext.OfficeAddress add = new OfficeAddress()
                    {
                        AddressLine = value.Address.AddresssLine,
                        Latitude = value.Address.Latitude,
                        Longitude = value.Address.Longitude,
                        OfficeId = OD.OfficeId,
                    };
                    context.OfficeAddresses.InsertOnSubmit(add);
                    context.SubmitChanges();

                    WalletDetail wallet = new WalletDetail()
                    {
                        OfficeId = OD.OfficeId,
                        Balance=0,
                    };
                    context.WalletDetails.InsertOnSubmit(wallet);
                    context.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {

                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = OD.OfficeId,
                    };
                }
            }
        }
        public Result GetCurrentOfficeDetail(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.OfficeDetails
                           where obj.UserId == UserId
                           select obj.OfficeId).SingleOrDefault();
                if (qes != null)
                {

                    return new Result()
                    {
                        Message = String.Format($"Get Current Office Details"),
                        Status = Result.ResultStatus.success,
                        Data = (from obj in context.OfficeDetails
                                join add in context.OfficeAddresses
                                on obj.OfficeId equals add.OfficeId
                                select new
                                {
                                    OfficeId = obj.OfficeId,
                                    OfficeName = obj.OfficeName,
                                    ManagerName = obj.ManagerName,
                                    PhoneNumber = obj.PhoneNumber,
                                    ImageId = obj.Image,
                                    Path = obj.Image == 0 ? null : obj.Image_Image.Path,
                                    CoverPhotoId = obj.CoverPhoto,
                                    Coverpath = obj.CoverPhoto == 0 ? null : obj.Image_CoverPhoto.Path,
                                    Description = obj.Description,
                                    UserId = obj.UserId,
                                    EmailId = obj.User.EmailId,
                                    Address = add.AddressLine
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
        public Result UpdateProfile(Model.AccountManager.OfficeDetail.UpdateProfile value, int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var qes = (from obj in context.OfficeDetails
                               where obj.UserId == UserId
                               select obj.OfficeId).SingleOrDefault();
                    OfficeDetail officeDetail = context.OfficeDetails.SingleOrDefault(x => x.OfficeId == qes);
                    if (officeDetail != null)
                    {
                        officeDetail.ManagerName = value.ManagerName;
                        officeDetail.PhoneNumber = value.PhoneNumber;
                        var I = context.Images.FirstOrDefault(i => i.ImageId == value.Image.Id);
                        if (I == null)
                        {
                            throw new ArgumentException("Invalid Image Id");
                        }
                        I.IsDeleted = false;
                        context.SubmitChanges();
                        officeDetail.Image = I.ImageId;

                        var C = context.Images.FirstOrDefault(c => c.ImageId == value.Cover.Id);
                        if (C != null)
                        {
                            C.IsDeleted = false;
                            context.SubmitChanges();
                        }
                        officeDetail.CoverPhoto = C.ImageId;
                        officeDetail.Description = value.Description;
                        context.SubmitChanges();


                        FoodCenterContext.OfficeAddress add = context.OfficeAddresses.FirstOrDefault(x => x.OfficeId == officeDetail.OfficeId);
                        if (add != null)
                        {
                            add.AddressLine = value.Address.AddresssLine;
                            add.Latitude = value.Address.Latitude;
                            add.Longitude = value.Address.Longitude;
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


                        return new Result()
                        {
                            Message = string.Format($"Profile Updated Successfully"),
                            Status = Result.ResultStatus.success,
                        };
                    }
                    else
                    {
                        return new Result()
                        {
                            Message = string.Format($"Invalid Office ID"),
                            Status = Result.ResultStatus.danger,
                        };
                    }
                }
            }
        }
    }
}
