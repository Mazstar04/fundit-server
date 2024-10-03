
using fundit_server.DTOs;
using fundit_server.Results;
using fundit_server.Wrapper;

namespace fundit_server.Interfaces.Services
{
    public interface ICampaignService
    {
        Task<BaseResponse> CreateCampaign(CreateCampaignRequest request);
        Task<PaginatedResult<GetCampaignResponse>> GetAllCampaigns(PaginationFilter paginationFilter);
        Task<PaginatedResult<GetCampaignResponse>> GetUserCampaigns(PaginationFilter paginationFilter);
        Task<PaginatedResult<GetCampaignResponse>> GetCampaignsByUserId(PaginationFilter paginationFilter, Guid userId);
        Task<BaseResponse<GetCampaignDetailResponse>> GetCampaignDetail(Guid campaignId);
       
    }
}