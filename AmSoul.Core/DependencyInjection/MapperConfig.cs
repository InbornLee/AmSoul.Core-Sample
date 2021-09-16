using AmSoul.Core.Models;
using AutoMapper;

namespace AmSoul.Core.DependencyInjection
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<BaseUser, UserDto>().ReverseMap();
            CreateMap<BaseRole, RoleDto>().ReverseMap();
        }
    }
}
