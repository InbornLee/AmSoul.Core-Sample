using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AmSoul.Core.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IUserService _userService;

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }

        protected virtual JwtSecurityToken Token
        {
            get
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return null;

                var authorization = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]).Parameter;

                return !string.IsNullOrEmpty(authorization) ? new JwtSecurityTokenHandler().ReadJwtToken(authorization) : null;
            }
        }

        protected virtual async Task<BaseUser> GetUser()
        {
            var tokenIdClaim = Token?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);

            return tokenIdClaim != null
                ? await _userService.GetUserByIdAsync(tokenIdClaim.Value)
                : null;
        }
    }
}