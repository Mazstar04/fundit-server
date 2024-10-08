using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Enums;

namespace fundit_server.MappingProfiles;
public class CampaignProfile : Profile
{
    public CampaignProfile()
    {

        CreateMap<CreateCampaignRequest, Campaign>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
           .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Campaign, GetCampaignResponse>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.FirstName}"))
          .ForMember(dest => dest.TotalPayment, opt => opt.MapFrom(src => src.Payments.Where(p=> p.Status == PaymentStatus.Successful).Count()))
        .ForMember(dest => dest.AmountRaised, opt => opt.MapFrom(src => src.Payments.Where(p=> p.Status == PaymentStatus.Successful).Sum(s=> s.Amount)));
        
        CreateMap<Campaign, GetCampaignDetailResponse>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.FirstName}"))
        .ForMember(dest => dest.TotalPayment, opt => opt.MapFrom(src => src.Payments.Where(p=> p.Status == PaymentStatus.Successful).Count()))
        .ForMember(dest => dest.AmountRaised, opt => opt.MapFrom(src => src.Payments.Where(p=> p.Status == PaymentStatus.Successful).Sum(s=> s.Amount)));
        // Add other mappings as needed...
    }
}
