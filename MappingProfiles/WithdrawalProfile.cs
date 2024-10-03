using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;

namespace fundit_server.MappingProfiles;
public class WithdrawalProfile : Profile
{
    public WithdrawalProfile()
    {

        CreateMap<WithdrawMoneyRequest, Withdrawal>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
           .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.UserId, opt => opt.Ignore())
           .ForMember(dest => dest.Status, opt => opt.Ignore());

        CreateMap<Withdrawal, GetWithdrawalResponse>();
        // Add other mappings as needed...
    }
}
