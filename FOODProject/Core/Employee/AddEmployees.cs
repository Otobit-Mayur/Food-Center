using FoodCenterContext;
using FOODProject.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FOODProject.Cores.Employee
{
    public class AddEmployees
    {
        public Result AddEmployee(Model.Employee.AddEmp value, int UserId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (FoodCenterDataContext context = new FoodCenterDataContext())
                {
                    var qes = (from obj in context.OfficeDetails
                               where obj.UserId == UserId
                               select obj.OfficeId).SingleOrDefault();

                    User u = new User();
                    u.EmailId = value.EmailId;
                    u.Password = value.Password;
                    u.RoleId = 3;
                    var res = context.Users.FirstOrDefault(x => x.EmailId == value.EmailId);

                    if (res != null)
                    {
                        throw new ArgumentException("Email Id Already Exist");
                    }
                    else
                    {
                        context.Users.InsertOnSubmit(u);
                        context.SubmitChanges();
                    }   

                    EmployeeDetail e = new EmployeeDetail();
                    e.UserId = u.UserId;
                    e.OfficeLocation = value.OfficeLocation;
                    e.OfficeId = qes;
                    
                    context.EmployeeDetails.InsertOnSubmit(e);
                    context.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = e.EmployeeId,
                    };
                }

            }
        }
       /* public Result AddEmpPassword(Model.Employee.AddEmployeePassword value, int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserId == Id);
                if (user != null)
                {
                    user.Password = value.Password;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Password Added"),
                        Status = Result.ResultStatus.success,
                        Data = user.UserId,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"User Not Found"),
                    Status = Result.ResultStatus.warning,
                };
            }
        }*/


        public Result AddEmpDetaile(Model.Employee.EmployeeDetail value,int Id)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                EmployeeDetail e = context.EmployeeDetails.FirstOrDefault(x => x.UserId == Id);
                if (e != null)
                {
                    e.EmployeeName = value.EmployeeName;
                    e.PhoneNumber = value.PhoneNumber;
                    e.IsDeleted = false;
                    e.IsActive = true;
                    var I = context.Images.FirstOrDefault(i => i.ImageId == value.Photo.Id);
                    if (I == null)
                    {
                        throw new ArgumentException("Invalid Image Id");
                    }
                    I.IsDeleted = false;
                    context.SubmitChanges();
                    e.Photo = I.ImageId;

                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"Details Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = e.EmployeeId,
                    };
                }
                return new Result()
                {
                    Message = string.Format($"User Not Found"),
                    Status = Result.ResultStatus.warning,
                    Data=e.EmployeeId
                };
            }
        }

        public Result GetCurrentEmployee(int UserId)
        {
            using (FoodCenterDataContext context = new FoodCenterDataContext())
            {
                var qes = (from obj in context.EmployeeDetails
                           where obj.UserId == UserId
                           select obj).SingleOrDefault();
                if (qes != null)
                {
                    return new Result()
                    {
                        Message = String.Format($"Get Current Employee Details"),
                        Status = Result.ResultStatus.success,
                        Data = (from obj in context.EmployeeDetails
                                where obj.EmployeeId==qes.EmployeeId
                                select new
                                {
                                    EmployeeId = obj.EmployeeId,
                                    EmployeeName = obj.EmployeeName,
                                    PhoneNo = obj.PhoneNumber,
                                    PhotoId = obj.Photo,
                                    Path = obj.Photo == 0 ? null : obj.Image_Photo.Path,
                                    CoverPhotoId = obj.CoverPhoto,
                                    CoverPath = obj.CoverPhoto == 0 ? null : obj.Image_CoverPhoto.Path,
                                    OfficeLocation = obj.OfficeLocation,
                                    IsActive = obj.IsActive,
                                    OfficeId = obj.OfficeId,
                                    UserId = obj.UserId,
                                    EmailId = obj.User.EmailId
                                }).FirstOrDefault(),
                    };
                }
            else
            {
                return new Result()
                {
                    Message = String.Format($"Invalid Current Employee"),
                    Status = Result.ResultStatus.success,
                };
            }
        }
        }
        public Result UpdateEmployeeDetail(Model.Employee.UpdateEmployeeDetail value,int UserId)
        {
            using(FoodCenterDataContext context=new FoodCenterDataContext())
            {
               
                var qes = (from obj in context.EmployeeDetails
                          where obj.UserId == UserId
                          select obj).FirstOrDefault();
                EmployeeDetail ED = context.EmployeeDetails.FirstOrDefault(x => x.EmployeeId == qes.EmployeeId);
                if (ED is not null)
                {
                    ED.EmployeeName = value.EmployeeName;
                    ED.PhoneNumber = value.PhoneNumber;
                    var I = context.Images.FirstOrDefault(i => i.ImageId == value.Photo.Id);
                    if (I != null)
                    {
                        ED.Photo= I.ImageId;
                        I.IsDeleted = false;
                        context.SubmitChanges();
                    }


                    var C = context.Images.FirstOrDefault(c => c.ImageId == value.CoverPhoto.Id);
                    if (C != null)
                    {
                        ED.CoverPhoto = C.ImageId;
                        C.IsDeleted = false;
                        context.SubmitChanges();
                    }
                    context.SubmitChanges();

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

                        Message = string.Format($"Invalid Employee ID"),
                        Status = Result.ResultStatus.danger,
                    };
                }
            }
        }
    }
}
