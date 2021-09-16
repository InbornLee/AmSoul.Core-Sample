using AmSoul.Core.Controllers;
using AmSoul.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VueJSDotnet51.Models;

namespace VueJSDotnet51.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : AsyncRESTControllerBase<Book>
    {
        public BookController(IUserService userService, IAsyncRESTService<Book> restService) : base(userService, restService) { }

    }
}
