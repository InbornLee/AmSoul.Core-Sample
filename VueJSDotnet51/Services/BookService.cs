using AmSoul.Core.Interfaces;
using AmSoul.Core.Services;
using VueJSDotnet51.Models;

namespace VueJSDotnet51.Services
{
    public class BookService : RESTServiceBase<Book>
    {
        public BookService(IDatabaseSettings settings) : base(settings) { }
    }
}
