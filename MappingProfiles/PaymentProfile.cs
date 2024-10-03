using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;

namespace fundit_server.MappingProfiles;
public class PaymentProfile : Profile
{
    public PaymentProfile()
    {

        CreateMap<MakePaymentRequest, Payment>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
           .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Payment, GetPaymentResponse>()
        .ForMember(dest => dest.CampaignTitle, opt => opt.MapFrom(src => src.Campaign.Title))
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserId == null ? "Anonymous" : $"{src.User.LastName} {src.User.FirstName}"))
        .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.User.ProfileImagePath));
        // Add other mappings as needed...
    }
}
