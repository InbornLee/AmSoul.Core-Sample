using AmSoul.Core.DependencyInjection;
using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmSoul.Core.Controllers
{
    public abstract class UserControllerBase : BaseController
    {
        private readonly IMapper _mapper;
        public UserControllerBase(IUserService userService, IMapper mapper)
          : base(userService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<UserDto>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var users = _userService
              .GetUsers()
              .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
              .ToList();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(BaseUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (Token == null) return Unauthorized();

            var user = await _userService.GetUserByIdAsync(id);

            return user != null
                ? Ok(_mapper.Map<UserDto>(user))
                : BadRequest();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await base.GetUser();
                if (user == null) return BadRequest("User_NotExit");
                var result = await _userService.ChangePassword(user, model.CurrentPwd.Trim(), model.NewPwd.Trim());
                if (result.Succeeded)
                {
                    return Ok("PWD_CHANGED_SUCCESS");
                }
                var error = IdentityExtensions.GetIdentityErrors(result);
                return BadRequest($"{error}");
            }
            return BadRequest("MODEL_STATE_ERROR");

        }
    }
}