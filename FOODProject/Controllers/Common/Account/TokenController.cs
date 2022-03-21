using Food_Center.Services;
using FoodCenterContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.Common.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly FoodCenterDataContext _context;

        public TokenController(TokenService tokenService,FoodCenterDataContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost]
        public IActionResult Refresh(Model.Common.Account.Token value)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(value.token);
            var emailid = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = _context.Users.SingleOrDefault(x => x.EmailId == emailid);
            var userrefresh = _context.UserRefreshes.SingleOrDefault(u => u.UserEmailId == emailid);
            var r1 = _context.RefreshTokens.SingleOrDefault(x => x.RefreshTokenId == userrefresh.RefreshTokenId);
            if (user == null || r1.RToken != value.refreshToken) return BadRequest();

            var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();


            r1.RToken = newRefreshToken;
            _context.SubmitChanges();

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
        [HttpPost("Revoke")]
        public IActionResult Revoke()
        {
            var emailaddress = User.Identity.Name;

            var userr = _context.Users.SingleOrDefault(u => u.EmailId == emailaddress);
            if (userr == null)
                return BadRequest();


            _context.SubmitChanges();

            return NoContent();
        }
    }
}
