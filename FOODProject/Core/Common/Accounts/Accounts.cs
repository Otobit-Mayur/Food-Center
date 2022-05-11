using Food_Center.Services;
using FoodCenterContext;
using FOODProject.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FOODProject.Core.Common.Accounts
{

    public class Accounts
    {
        FoodCenterDataContext context = new FoodCenterDataContext();
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public Accounts(IConfiguration configuration, TokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public Result AddRole(Model.Common.Account.Role value)
        {
            Role role = new Role();

            role.RoleName = char.ToUpper(value.role[0]) + value.role.Substring(1).ToLower();
            var check = context.Roles.FirstOrDefault(x => x.RoleName == value.role);
            if (check != null)
            {
                return new Result()
                {
                    Message = string.Format("Role Already Exits"),
                    Status = Result.ResultStatus.danger,
                };
            }
            else
            {
                context.Roles.InsertOnSubmit(role);
                context.SubmitChanges();
                //return "New Role Added Successfully";
                return new Result()
                {
                    Message = string.Format("New Role Added Successfully"),
                    Status = Result.ResultStatus.none,
                    Data = value.role,
                };
            }
        }
        public Result GetallRole()
        {
            //var qs = (from obj in context.Roles
                      //select new { obj.RoleId, obj.RoleName }).Take(2);
            return new Result()
            {
                Message = string.Format("New Role Added Successfully"),
                Status = Result.ResultStatus.none,
                Data = (from obj in context.Roles
                        select new Model.Common.IntegerNullString()
                        {
                          Id=obj.RoleId,
                          Text=obj.RoleName
                        }).Take(2)
            };


        }

        public Result SignUp(Model.Common.Account.User values)
        {
            User sp = new User();
            Role role = new Role();
         
            var ms = context.Roles.SingleOrDefault(c => c.RoleName == values.Role.Text);
           
            var res = context.Users.FirstOrDefault(x => x.EmailId == values.EmailId);
            if (res != null)
            {
                //throw new ArgumentException("Email Id Already Exist");
                return new Result()
                {
                    Message = string.Format($"Email Id Already Exist"),
                    Status = Result.ResultStatus.warning,
                };
            }
            else
            {
                sp.EmailId = values.EmailId;
                sp.Password = values.Password;
                sp.RoleId = ms.RoleId;
                
                context.Users.InsertOnSubmit(sp);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{values.EmailId} Signup Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = new Model.Common.IntegerNullString()
                    {
                        Id = sp.UserId,
                        Text = sp.EmailId
                    }
                };
                    /*return "Signup Successfully";*/
            }
        }
        public Result CheckEmail(Model.Common.Account.CheckEmail value)
        {
            var res = context.Users.FirstOrDefault(x => x.EmailId == value.EmailId);
            if (res != null)
            {
                return new Result()
                {
                    //throw new ArgumentException("EmailId Already Exists");
                    Message = string.Format($"EmailId Already Exists"),
                    Status = Result.ResultStatus.warning,
                    Data = "False",
                };
            }
            return new Result()
            {
                Message = string.Format($"Success"),
                Status = Result.ResultStatus.success,
                Data="True",
            };
        }
        public Result getallUser()
        {
            return new Result()
            {
                Message = string.Format("Get All User Successfully"),
                Status = Result.ResultStatus.none,
                Data = (from obj in context.Users
                        select new { obj.UserId, obj.EmailId, obj.RoleId }).ToList(),
            };
            
        }

        public Result Login(Model.Common.Account.Login values)
        {

            var result = (from SignUp in context.Users
                          where SignUp.EmailId == values.EmailId && SignUp.Password == values.Password
                          select new 
                          {
                              UserId=SignUp.UserId,
                              EmailId=SignUp.EmailId,
                              RoleId=SignUp.RoleId
                          }).FirstOrDefault();


            //Query To Get Current User Login 
            var qs = (from obj in context.Users
                      where obj.EmailId == values.EmailId
                      select obj.UserId).SingleOrDefault();

            if (result != null)
            {
                var authclaims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name,result.EmailId),
                     new Claim(ClaimTypes.Sid,result.UserId.ToString()),
                     new Claim(ClaimTypes.Role,result.RoleId.ToString()),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var jwtToken = _tokenService.GenerateAccessToken(authclaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                RefreshToken rt = new RefreshToken();   
                rt.RToken = refreshToken;
                context.RefreshTokens.InsertOnSubmit(rt);
                context.SubmitChanges();

                UserRefresh ur = new UserRefresh();
                ur.UserEmailId = result.EmailId;
                ur.RefreshTokenId = rt.RefreshTokenId;
                context.UserRefreshes.InsertOnSubmit(ur);
                context.SubmitChanges();

                if (result.RoleId == 1)
                {
                    var q = (from Shop in context.ShopDetails
                             where Shop.UserId == result.UserId
                             select Shop.UserId).Count();
                    if (q != 1)
                    {
                        return new Result()
                        {
                            Message = string.Format("Shop Detail Not Found"),
                            Status = Result.ResultStatus.none,
                            Data = new
                            {
                                UserId = result.UserId,
                                RoleId = result.RoleId
                            },
                        };
                    }
                }
                if (result.RoleId == 2)
                {
                    var q = (from office in context.OfficeDetails
                             where office.UserId == result.UserId
                             select office.UserId).Count();
                    if (q != 1)
                    {
                        return new Result()
                        {
                            Message = string.Format("Office Detail Not Found"),
                            Status = Result.ResultStatus.none,
                            Data  = new
                            {
                                UserId = result.UserId,
                                RoleId=result.RoleId
                            },
                        };
                    }
                }
                if (result.RoleId == 3)
                {
                    var check = (from Employee in context.EmployeeDetails
                                 where Employee.UserId == result.UserId && Employee.IsDeleted == true
                                 select Employee.UserId).FirstOrDefault();
                    if (check is not null)
                    {
                        throw new ArgumentException("Employee Deleted");
                    }


                    var qa = (from Employee in context.EmployeeDetails
                              where Employee.UserId == result.UserId
                              select Employee).FirstOrDefault();

                    if (qa.EmployeeName is null)
                    {
                        return new Result()
                        {
                            Message = string.Format("Employee Detail Not Found"),
                            Status = Result.ResultStatus.none,
                            Data = new
                            {
                                UserId = result.UserId,
                                EmployeeId=qa.EmployeeId,
                                RoleId = result.RoleId
                            },
                        };
                    }
                }

                return new Result()
                {
                    Message = String.Format($"Login Successfully"),
                    Status=Result.ResultStatus.success,
                    Data=new
                    {
                        token=jwtToken,
                        RefreshToken=refreshToken,
                        UserId=result.UserId,
                        UserEmailId= result.EmailId,
                        RoleId=result.RoleId,

                    },
                };
            }
            else
            {
                return new Result()
                {
                    Message = string.Format("Please Enter Valid Login Details"),
                    Status = Result.ResultStatus.none,
                };
                //throw new ArgumentException("Please Enter Valid Login Details..");
            }
        }
        public Result getsid(string Emailid)
        {
            var uid = (from user in context.Users
                       where user.EmailId == Emailid
                       select user.UserId).SingleOrDefault();
            return new Result()
            {
                Message = string.Format($"{uid} is Current User"),
                Status = Result.ResultStatus.success,
                Data = uid,
            };
        }
       
        public double CalculateDistance(Location point1)
        {
            var d1 = point1.Latitude1 * (Math.PI / 180.0);
            var num1 = point1.Longitude1 * (Math.PI / 180.0);
            var d2 = point1.Latitude2 * (Math.PI / 180.0);
            var num2 = point1.Longitude2 * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return Math.Round(6371.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }

        public class Location
        {
            public double Latitude1 = 21.1481604;
            public double Longitude1 = 72.7730543;
            public double Latitude2 = 21.1583979;
            public double Longitude2 = 72.837367;

        }

        //21.1481604,72.7730543
        //21.1481486,72.7600902
        //21.1484738,72.7550798
        //21.1583979,72.837367


    }
}
