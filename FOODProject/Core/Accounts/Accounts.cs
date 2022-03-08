using Food_Center.Services;
using FoodCenterContext;
using FOODProject.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FOODProject.Core.Accounts
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

        public Result AddRole(Model.Account.Role value)
        {
            Role role = new Role();

            role.Role1 = value.role;
            var check = context.Roles.FirstOrDefault(x => x.Role1 == value.role);
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

     
        public async Task<IEnumerable> get()
        {
            var qs = (from obj in context.Roles
                      select new { obj.RoleId, obj.Role1 }).ToList();
            return qs;
        }

        public Result SignUp(Model.Account.User values)
        {
            User sp = new User();
            Role role = new Role();
         
            var ms = context.Roles.SingleOrDefault(c => c.Role1 == values.RoleId.String);
           
            var res = context.Users.FirstOrDefault(x => x.EmailId == values.EmailId);
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

        public Result Login(Model.Account.Login values)
        {

            var result = (from SignUp in context.Users
                          where SignUp.EmailId == values.EmailId && SignUp.Password == values.Password
                          select SignUp).ToList();

            if (result.Count() == 1)
            {

                var authclaims = new List<Claim>
                  {
                     new Claim(ClaimTypes.Name,values.EmailId),
                     /*new Claim(ClaimTypes.Sid,values.UserId),*/
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                  };

                var jwtToken = _tokenService.GenerateAccessToken(authclaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                RefreshToken rt = new RefreshToken();
                rt.RToken = refreshToken;
                context.RefreshTokens.InsertOnSubmit(rt);
                context.SubmitChanges();

                UserRefresh ur = new UserRefresh();
                ur.UserEmailId = values.EmailId;
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
                        UserEmailId=values.EmailId,
                    },
                };
            }
            else
            {
                throw new ArgumentException("Please Enter Valid Login Details..");
            }
        }

        /*public async Task<IEnumerable> GetEmailId()
        {
            var Email=(string)HttpContext.Items["EmailId"];
            var qs = (from obj in context.Roles
                      select new { obj.RoleId, obj.Role1 }).ToList();
            return qs;
        }*/

    }
}
