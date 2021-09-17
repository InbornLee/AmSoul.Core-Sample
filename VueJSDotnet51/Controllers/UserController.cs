using AmSoul.Core.Controllers;
using AmSoul.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VueJSDotnet51.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : UserControllerBase
    {
        public UserController(IUserService userService, IMapper mapper)
          : base(userService, mapper)
        { }

    }
}