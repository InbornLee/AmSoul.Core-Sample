using AmSoul.Core.Controllers;
using AmSoul.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VueJSDotnet51.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    [AllowAnonymous]
    public class AuthenticationController : AuthenticationControllerBase
    {
        public AuthenticationController(IUserService userService, IMapper mapper)
          : base(userService, mapper)
        { }
    }
}