using AmSoul.Core.Controllers;
using AmSoul.Core.DependencyInjection;
using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace VueJSDotnet51.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class AdminController : BaseController
    {
        private readonly IMapper _mapper;
        public AdminController(IUserService userService, IMapper mapper)
          : base(userService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("Role/Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> RegisterRole([FromForm] RoleDto request)
        {
            if (string.IsNullOrEmpty(request.Name)) return UnprocessableEntity("RoleName not allowed null");
            try
            {
                var role = _mapper.Map<BaseRole>(request);
                var result = await _userService.CreateRoleAsync(role);
                if (result.Succeeded)
                    return Ok(_mapper.Map<RoleDto>(role));
                else
                    return UnprocessableEntity(IdentityExtensions.GetIdentityErrors(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unknown error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("User/Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> RegisterUser([FromForm] UserDto request)
        {
            //if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            //{
            //    return UnprocessableEntity("Email & pasword are required");
            //}

            try
            {
                var user = new BaseUser()
                {
                    Email = request.Email,
                    UserName = request.UserName
                };
                var result = await _userService.CreateAsync(user, request.Password, request.Roles);
                if (result.Succeeded)
                    return CreatedAtRoute("GetUserById", new { id = user.Id }, null);

                return UnprocessableEntity(IdentityExtensions.GetIdentityErrors(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unknown error: {ex.Message}");
            }
        }

    }
}
