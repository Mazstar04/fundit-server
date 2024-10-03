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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IUserService _userService;
        private readonly ICampaignRepository _campaignRepo;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepo, IUserService userService, IUserIdentityService userIdentityService, IMapper mapper, ICampaignRepository campaignRepo)
        {
            _paymentRepo = paymentRepo;
            _userService = userService;
            _userIdentityService = userIdentityService;
            _mapper = mapper;
            _campaignRepo = campaignRepo;
        }

        public async Task<BaseResponse> MakePayment(MakePaymentRequest request)
        {

            var campaign = await _campaignRepo.GetAsync(request.CampaignId);
            if (campaign == null)
            {
                return new BaseResponse
                {
                    Succeeded = false,
                    Message = "Campaign not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var newPayment = _mapper.Map<Payment>(request);
            newPayment.UserId = request.UserId;
            await _paymentRepo.AddAsync(newPayment);
            await _userService.FundWallet(campaign.UserId, request.Amount);

            await _paymentRepo.SaveChangesAsync();

            return new BaseResponse
            {
                Succeeded = true,
                Message = "Payment successful",
                StatusCode = HttpStatusCode.OK,
            };
        }

        public async Task<PaginatedResult<GetPaymentResponse>> GetPaymentsByCampaignId(PaginationFilter paginationFilter, Guid campaignId)
        {
            var query = _paymentRepo.Query()
               .Include(t => t.User)
                .Include(t => t.Campaign)
               .Where(p => p.CampaignId == campaignId);

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.User.FirstName.Contains(paginationFilter.SearchValue)
                                      || c.User.LastName.Contains(paginationFilter.SearchValue)
                                      || c.Campaign.Title.Contains(paginationFilter.SearchValue));
            }
            var totalCount = await query.CountAsync();
            var payments = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)

               .ToListAsync();

            var paymentResponse = _mapper.Map<List<GetPaymentResponse>>(payments);

            return new PaginatedResult<GetPaymentResponse>
            {
                Data = paymentResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Payment records fetched successfully" }
            };
        }

        public async Task<PaginatedResult<GetPaymentResponse>> GetUserIncomingPayments(PaginationFilter paginationFilter)
        {
            var userId = _userIdentityService.GetUserId();
            var query = _paymentRepo.Query()
                .Include(t => t.User)
               .Include(t => t.Campaign)
               .ThenInclude(t => t.User)
                .Where(p => p.Campaign.UserId == userId);

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.User.FirstName.Contains(paginationFilter.SearchValue)
                                      || c.User.LastName.Contains(paginationFilter.SearchValue)
                                      || c.Campaign.Title.Contains(paginationFilter.SearchValue));
            }
            var totalCount = await query.CountAsync();
            var payments = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)

               .ToListAsync();

            var paymentResponse = _mapper.Map<List<GetPaymentResponse>>(payments);

            return new PaginatedResult<GetPaymentResponse>
            {
                Data = paymentResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Payment records fetched successfully" }
            };
        }

        public async Task<PaginatedResult<GetPaymentResponse>> GetUserOutgoingPayments(PaginationFilter paginationFilter)
        {
            var userId = _userIdentityService.GetUserId();
            var query = _paymentRepo.Query()
                .Include(t => t.User)
                .Include(t => t.Campaign)
                .Where(p => p.UserId == userId);

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.User.FirstName.Contains(paginationFilter.SearchValue)
                                      || c.User.LastName.Contains(paginationFilter.SearchValue)
                                      || c.Campaign.Title.Contains(paginationFilter.SearchValue));
            }
            var totalCount = await query.CountAsync();
            var payments = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)

               .ToListAsync();

            var paymentResponse = _mapper.Map<List<GetPaymentResponse>>(payments);

            return new PaginatedResult<GetPaymentResponse>
            {
                Data = paymentResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Payment records fetched successfully" }
            };
        }



    }
}