using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Enums;

namespace fundit_server.MappingProfiles;
public class PaymentProfile : Profile
{
    public PaymentProfile()
    {

        CreateMap<InitializePaymentRequest, Payment>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
           .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
           .ForMember(dest => dest.Reference, opt => opt.Ignore());

        CreateMap<Payment, GetPaymentResponse>()
        .ForMember(dest => dest.CampaignTitle, opt => opt.MapFrom(src => src.Campaign.Title));
        // Add other mappings as needed...
    }
}
