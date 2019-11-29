using AutoMapper;
#if (!useMongoDB)
using RootNamespace.Entities.Domain;
#else
using RootNamespace.Entities.Domain.Mongo;
#endif
using RootNamespace.Entities.DTO;

namespace RootNamespace.Entities.AutoMapper.Profiles
{
    public class UserMappings : Profile, IMapperMarker
    {
        public UserMappings()
        {
            // Create bidirectional mapping from User -> UserDTO
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}