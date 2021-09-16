using AmSoul.Core.Interfaces;
using AmSoul.Core.Services;
using VueJSDotnet51.Models;

namespace VueJSDotnet51.Services
{
    public class TeamService : RESTServiceBase<Team>
    {
        public TeamService(IDatabaseSettings settings) : base(settings) { }
    }
}
