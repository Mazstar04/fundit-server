using System.Net;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Enums;
using fundit_server.Interfaces.Repositories;
using fundit_server.Interfaces.Services;
using fundit_server.Results;
using fundit_server.Wrapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace fundit_server.Implementations.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IUserService _userService;
        private readonly ICampaignRepository _campaignRepo;
        private readonly HttpClient _httpClient;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepo, IUserService userService, IUserIdentityService userIdentityService, IMapper mapper, ICampaignRepository campaignRepo, HttpClient httpClient, IConfiguration config)
        {
            _paymentRepo = paymentRepo;
            _userService = userService;
            _userIdentityService = userIdentityService;
            _mapper = mapper;
            _campaignRepo = campaignRepo;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<BaseResponse<string>> InitializePayment(InitializePaymentRequest request)
        {
            var campaign = await _campaignRepo.GetAsync(request.CampaignId);
            if (campaign == null)
            {
                return new BaseResponse<string>
                {
                    Succeeded = false,
                    Message = "Campaign not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            // Prepare payment request to Paystack
            var secretKey = _config["Paystack:SecretKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);

            var reference = Guid.NewGuid().ToString();

            var paymentRequest = new
            {
                amount = request.Amount * 100, // Paystack expects amount in kobo (cents)
                email = request.Email,
                reference = reference, 
                callback_url = $"{_config["Paystack:CallbackUrl"]}" 
            };

            var content = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_config["Paystack:ApiBaseUri"]}/transaction/initialize", content);

            if (!response.IsSuccessStatusCode)
            {
                return new BaseResponse<string>
                {
                    Succeeded = false,
                    Message = "Payment initialization failed",
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }

            var paymentResponse = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            // Create a new payment record with 'Pending' status
            var newPayment = _mapper.Map<Payment>(request);
            newPayment.Reference = reference;

            await _paymentRepo.AddAsync(newPayment);
            await _paymentRepo.SaveChangesAsync();

            // Return payment URL (redirect to Paystack)
            return new BaseResponse<string>
            {
                Succeeded = true,
                Message = "Payment initialized successfully",
                StatusCode = HttpStatusCode.OK,
                Data = paymentResponse.data.authorization_url // URL to redirect the user for payment
            };
        }

        public async Task<BaseResponse> PaymentCallback(string reference)
        {

            var paymentResult = await VerifyPaymentResult(reference);
            var query = _paymentRepo.Query();

            var payment = await query
                .Where(c => c.Reference == reference)
                .Include(t => t.Campaign)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return new BaseResponse
                {
                    Succeeded = false,
                    Message = "Payment record not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }
             if (payment.Status == PaymentStatus.Successful)
            {
                return new BaseResponse
                {
                    Succeeded = true,
                    Message = "Payment already verified",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            
            if (paymentResult.status == "cancelled" || paymentResult.status == "failed")
            {
                // Update the payment status to 'Cancelled'
                payment.Status = paymentResult.status == "cancelled" ? PaymentStatus.Cancelled : PaymentStatus.Failed;
                await _paymentRepo.UpdateAsync(payment);
                await _paymentRepo.SaveChangesAsync();

                return new BaseResponse
                {
                    Succeeded = false,
                    Message = paymentResult.status == "cancelled" ? "Payment was cancelled " : "Payment Failed",
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            if (paymentResult.status == "success")
            {
                payment.Status = PaymentStatus.Successful;
                await _paymentRepo.UpdateAsync(payment);
                await _userService.FundWallet(payment.Campaign.UserId, payment.Amount);
                await _paymentRepo.SaveChangesAsync();
                return new BaseResponse
                {
                    Succeeded = true,
                    Message = "Payment verified successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }

            return new BaseResponse
            {
                Succeeded = false,
                Message = "Payment verification failed",
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        private async Task<dynamic> VerifyPaymentResult(string reference)
        {
            var secretKey = _config["Paystack:SecretKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);

            var response = await _httpClient.GetAsync($"{_config["Paystack:ApiBaseUri"]}/transaction/verify/{reference}");

            if (!response.IsSuccessStatusCode) return null;

            var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            return result.data; // Return the full result to check status
        }

        public async Task<PaginatedResult<GetPaymentResponse>> GetPaymentsByCampaignId(PaginationFilter paginationFilter, PaymentStatus? status, Guid campaignId)
        {
            var query = _paymentRepo.Query()
                .Include(t => t.Campaign)
               .Where(p => p.CampaignId == campaignId);

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }


            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.Username.Contains(paginationFilter.SearchValue)
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

        public async Task<PaginatedResult<GetPaymentResponse>> GetUserPayments(PaginationFilter paginationFilter, PaymentStatus? status)
        {
            var userId = _userIdentityService.GetUserId();
            var query = _paymentRepo.Query()
               .Include(t => t.Campaign)
               .ThenInclude(t => t.User)
                .Where(p => p.Campaign.UserId == userId);

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.Username.Contains(paginationFilter.SearchValue)
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