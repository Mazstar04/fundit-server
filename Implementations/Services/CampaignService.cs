using System.Net;
using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;
using fundit_server.Interfaces.Services;
using fundit_server.Results;
using fundit_server.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace fundit_server.Implementations.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IMapper _mapper;

        public CampaignService(ICampaignRepository campaignRepo, IMapper mapper, IUserIdentityService userIdentityService)
        {
            _campaignRepo = campaignRepo;
            _mapper = mapper;
            _userIdentityService = userIdentityService;
        }


        public async Task<BaseResponse> CreateCampaign(CreateCampaignRequest request)
        {

            var campaign = _mapper.Map<Campaign>(request);
            campaign.UserId = _userIdentityService.GetUserId();

            try
            {
                await _campaignRepo.AddAsync(campaign);
                await _campaignRepo.SaveChangesAsync();

                return new BaseResponse
                {
                    Succeeded = true,
                    Message = "Campaign created successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Succeeded = false,
                    Message = $"An error occurred while creating the campaign: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<PaginatedResult<GetCampaignResponse>> GetAllCampaigns(PaginationFilter paginationFilter)
        {
            var query = _campaignRepo.Query();

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.Title.Contains(paginationFilter.SearchValue)
                                      || c.ShortDescription.Contains(paginationFilter.SearchValue));
            }
            var totalCount = await query.CountAsync();
            var trips = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)
               .Include(t => t.User)
               .Include(t => t.Payments)
               .ToListAsync();

            var campaignResponse = _mapper.Map<List<GetCampaignResponse>>(trips);

            return new PaginatedResult<GetCampaignResponse>
            {
                Data = campaignResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Campaigns fetched successfully" }
            };
        }

        public async Task<PaginatedResult<GetCampaignResponse>> GetCampaignsByUserId(PaginationFilter paginationFilter, Guid userId)
        {
            var query = _campaignRepo.Query().Where(c => c.UserId == userId);

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.Title.Contains(paginationFilter.SearchValue)
                                      || c.ShortDescription.Contains(paginationFilter.SearchValue));
            }
            var totalCount = await query.CountAsync();
            var trips = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)
               .Include(t => t.User)
               .Include(t => t.Payments)
               .ToListAsync();

            var campaignResponse = _mapper.Map<List<GetCampaignResponse>>(trips);

            return new PaginatedResult<GetCampaignResponse>
            {
                Data = campaignResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Campaigns fetched successfully" }
            };
        }

        public async Task<PaginatedResult<GetCampaignResponse>> GetUserCampaigns(PaginationFilter paginationFilter)
        {
            var userId = _userIdentityService.GetUserId();
            var query = _campaignRepo.Query().Where(c => c.UserId == userId);

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.Title.Contains(paginationFilter.SearchValue)
                                      || c.ShortDescription.Contains(paginationFilter.SearchValue));
            }
            var totalCount = await query.CountAsync();
            var trips = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)
               .Include(t => t.User)
               .Include(t => t.Payments)
               .ToListAsync();

            var campaignResponse = _mapper.Map<List<GetCampaignResponse>>(trips);

            return new PaginatedResult<GetCampaignResponse>
            {
                Data = campaignResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Campaigns fetched successfully" }
            };
        }

        public async Task<BaseResponse<GetCampaignDetailResponse>> GetCampaignDetail(Guid campaignId)
        {
            var query = _campaignRepo.Query();

            var campaign = await query
                .Where(c => c.Id == campaignId)
                .Include(t => t.User)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync();

            if (campaign == null)
            {
                return new BaseResponse<GetCampaignDetailResponse>
                {
                    Succeeded = false,
                    Message = "Campaign not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var campaignResponse = _mapper.Map<GetCampaignDetailResponse>(campaign);
            return new BaseResponse<GetCampaignDetailResponse>
            {
                Data = campaignResponse,
                Succeeded = true,
                Message = "Campaign detail fetched successfull",
                StatusCode = HttpStatusCode.OK
            };

        }


    }
}