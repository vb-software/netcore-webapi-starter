using AutoMapper;
using RootNamespace.Entities.Domain;
using RootNamespace.Entities.DTO;

namespace RootNamespace.Entities.AutoMapper.Profiles
{
    public class UserMappings : Profile, IMapperMarker
    {
        public UserMappings()
        {
            // Create bidirectional mapping from User -> UserDTO
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}