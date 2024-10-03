using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;

namespace fundit_server.MappingProfiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
    
         CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.HashSalt, opt => opt.Ignore())
            .ForMember(dest => dest.WalletBalance, opt => opt.MapFrom(src => 0));

           
        // Add other mappings as needed...
    }
}
