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
            return new Result()
            {
                Message = string.Format("Get All  Role Successfully"),
                Status = Result.ResultStatus.none,
                Data =(from obj in context.Roles
                       select new { obj.RoleId, obj.RoleName }).ToList(),
            };
            
      
        }

        public Result SignUp(Model.Common.Account.User values)
        {
            User sp = new User();
            Role role = new Role();
         
            var ms = context.Roles.SingleOrDefault(c => c.RoleName == values.RoleId.String);
           
            var res = context.Users.FirstOrDefault(x => x.EmailId == values.EmailId && x.RoleId==ms.RoleId);
            if (res != null)
            {
                throw new ArgumentException("Email Id Already Exist");
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
                    Data=sp.UserId,
                };
                /*return "Signup Successfully";*/
            }

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

            if(result.RoleId==1)
            {
                var q = (from Shop in context.ShopDetails
                         where Shop.UserId == result.UserId
                         select Shop.UserId).Count();
                if (q != 1)
                {
                    throw new ArgumentException("Details Not Found ");
                }
            }
            if (result.RoleId == 2)
            {
                var q = (from office in context.OfficeDetails
                         where office.UserId == result.UserId
                         select office.UserId).Count();
                if (q != 1)
                {
                    throw new ArgumentException("Details Not Found ");
                }
            }
            if (result.RoleId == 3)
            {
                var q = (from Employee in context.EmployeeDetails
                         where Employee.UserId == result.UserId
                         select Employee.UserId).Count();
                if (q != 1)
                {
                    throw new ArgumentException("Details Not Found ");
                }
            }

            //Check User Details 
            /*var q = (from Shop in context.ShopDetails
                     from Office in context.OfficeDetails
                     from Employee in context.EmployeeDetails
                     where Shop.UserId == result.UserId || Office.UserId == result.UserId || Employee.UserId == result.UserId
                     select Shop.UserId).Count();

            if (q!=1)
            {
                throw new ArgumentException("Details Not Found ");
            }*/


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

                return new Result()
                {
                    Message = String.Format($"Login Successfully"),
                    Status=Result.ResultStatus.success,
                    Data=new
                    {
                        token=jwtToken,
                        RefreshToken=refreshToken,
                        UserEmailId= result.EmailId,
                        RoleId=result.RoleId,

                    },
                };
            }
            else
            {
                throw new ArgumentException("Please Enter Valid Login Details..");
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
