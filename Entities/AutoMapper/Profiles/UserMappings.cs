using AutoMapper;
using Entities.Domain;
using Entities.DTO;

namespace Entities.AutoMapper.Profiles
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