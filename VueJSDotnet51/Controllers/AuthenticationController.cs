using AmSoul.Core.Controllers;
using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace VueJSDotnet51.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IMapper _mapper;
        public AuthenticationController(IUserService userService, IMapper mapper)
          : base(userService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("Role")]
        public IActionResult GetRoles()
        {
            var roles = _userService
                .GetRoles()
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .ToList();
            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromForm] UserDto request)
        {
            if (string.IsNullOrEmpty(request.UserName))
                return UnprocessableEntity("UserName are required");
            //if (string.IsNullOrEmpty(request.Email))
            //    return UnprocessableEntity("Email are required");
            if (string.IsNullOrEmpty(request.Password))
                return UnprocessableEntity("Password are required");

            var user = await _userService.GetUserByNameAsync(request.UserName);
            AuthenticationResult response = new() { };
            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            if (await _userService.CheckPasswordAsync(user, request.Password))
            {
                var token = await _userService.CreateAuthorizationToken(user);
                response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                response.Success = true;

                return Ok(response);
            }

            return BadRequest("Password Wrong");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Me")]
        [ProducesResponseType(typeof(BaseUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Me()
        {
            if (Token != null)
            {
                var tokenIdClaim = Token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);
                if (tokenIdClaim != null)
                {
                    var user = await _userService.GetUserByIdAsync(tokenIdClaim.Value);
                    return user != null
                        ? Ok(_mapper.Map<UserDto>(user))
                        : BadRequest();
                }
            }
            return Unauthorized();
        }
    }
}