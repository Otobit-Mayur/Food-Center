using Food_Center.Services;
using FoodCenterContext;
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

        public async Task<string> AddRole(Model.Account.Role value)
        {
            Role role = new Role();

            role.Role1 = value.role;
            var check = context.Roles.FirstOrDefault(x => x.Role1 == value.role);
            if (check != null)
            {
                return "Role already exist";
            }
            else
            {
                context.Roles.InsertOnSubmit(role);
                context.SubmitChanges();
                return "New Role Added Successfully";
            }
        }

     
        public async Task<IEnumerable> get()
        {
            var qs = (from obj in context.Roles
                      select new { obj.RoleId, obj.Role1 }).ToList();
            return qs;
        }

        public async Task<string> SignUp(Model.Account.User values)
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
                return "SignUp Successfully";
            }

        }

        public async Task<string> Login(Model.Account.Login values)
        {

            var result = (from SignUp in context.Users
                          where SignUp.EmailId == values.EmailId && SignUp.Password == values.Password
                          select SignUp).ToList();

            if (result.Count() == 1)
            {

                var authclaims = new List<Claim>
                  {
                 new Claim(ClaimTypes.Name,values.EmailId),
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

                return jwtToken;
            }
            else
            {
                return "Please Enter valid login details";
            }
        }

    }
}
