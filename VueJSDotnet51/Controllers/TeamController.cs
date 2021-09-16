using AmSoul.Core.Controllers;
using AmSoul.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VueJSDotnet51.Models;

namespace VueJSDotnet51.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : AsyncRESTControllerBase<Team>
    {
        public TeamController(IUserService userService, IAsyncRESTService<Team> restService) : base(userService, restService) { }
    }
}
