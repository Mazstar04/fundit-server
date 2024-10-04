using System.Net;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;
using fundit_server.Interfaces.Services;
using fundit_server.Results;
using fundit_server.Wrapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace fundit_server.Implementations.Services
{


    public class WithdrawalService : IWithdrawalService
    {
        private readonly IWithdrawalRepository _withdrawalRepo;
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public WithdrawalService(IMapper mapper, IUserIdentityService userIdentityService, IUserService userService, IWithdrawalRepository withdrawalRepo, IUserRepository userRepo, IConfiguration config, HttpClient httpClient)
        {
            _mapper = mapper;
            _userIdentityService = userIdentityService;
            _userService = userService;
            _withdrawalRepo = withdrawalRepo;
            _userRepo = userRepo;
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<BaseResponse> WithdrawMoney(WithdrawMoneyRequest request)
        {
            var userId = _userIdentityService.GetUserId();
            var user = await _userRepo.GetAsync(userId);
            if (user == null)
            {
                return new BaseResponse
                {
                    Succeeded = false,
                    Message = "user not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            if (user.WalletBalance < request.Amount)
            {
                return new BaseResponse
                {
                    Succeeded = false,
                    Message = "Insufficient balance",
                    StatusCode = HttpStatusCode.Conflict,
                };
            }


            var newWithdrawal = _mapper.Map<Withdrawal>(request);
            newWithdrawal.UserId = userId;

            await _withdrawalRepo.AddAsync(newWithdrawal);
            await _userService.DeductFromWallet(userId, request.Amount);

            await _withdrawalRepo.SaveChangesAsync();

            return new BaseResponse
            {
                Succeeded = true,
                Message = "Withdrawal successful",
                StatusCode = HttpStatusCode.OK,
            };
        }


        public async Task<PaginatedResult<GetWithdrawalResponse>> GetUserWithdrawals(PaginationFilter paginationFilter)
        {
            var userId = _userIdentityService.GetUserId();
            var query = _withdrawalRepo.Query()
                .Where(p => p.UserId == userId);

            // Apply search if SearchValue is provided
            if (!string.IsNullOrWhiteSpace(paginationFilter.SearchValue))
            {
                query = query.Where(c => c.AccountNo.Contains(paginationFilter.SearchValue)
                                      || c.BankName.Contains(paginationFilter.SearchValue)
                                      || c.AccountName.Contains(paginationFilter.SearchValue));
            }

            var totalCount = await query.CountAsync();
            var withdrawals = await query
            .OrderByDescending(t => t.Created)
               .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
               .Take(paginationFilter.PageSize)

               .ToListAsync();

            var withdrawalResponse = _mapper.Map<List<GetWithdrawalResponse>>(withdrawals);

            return new PaginatedResult<GetWithdrawalResponse>
            {
                Data = withdrawalResponse,
                CurrentPage = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationFilter.PageSize),
                Succeeded = true,
                Messages = new List<string> { "Withdrawal records fetched successfully" }
            };
        }

        // would work if i have registered usiness
        //  public async Task<BaseResponse> WithdrawMoney(WithdrawMoneyRequest request)
        //         {
        //             var userId = _userIdentityService.GetUserId();
        //             var user = await _userRepo.GetAsync(userId);
        //             if (user == null)
        //             {
        //                 return new BaseResponse
        //                 {
        //                     Succeeded = false,
        //                     Message = "User not found",
        //                     StatusCode = HttpStatusCode.NotFound,
        //                 };
        //             }

        //             if (user.WalletBalance < request.Amount)
        //             {
        //                 return new BaseResponse
        //                 {
        //                     Succeeded = false,
        //                     Message = "Insufficient balance",
        //                     StatusCode = HttpStatusCode.Conflict,
        //                 };
        //             }

        //             // Step 1: Resolve Account Number
        //             var accountResolveUrl = $"{_config["Paystack:ApiBaseUri"]}/bank/resolve?account_number={request.AccountNo}&bank_code={request.BankCode}";

        //             _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["Paystack:SecretKey"]);

        //             var resolveResponse = await _httpClient.GetAsync(accountResolveUrl);
        //             var resolveResult = JsonConvert.DeserializeObject<ResolveAccountResponse>(await resolveResponse.Content.ReadAsStringAsync());
        //             if (!resolveResponse.IsSuccessStatusCode || !resolveResult.Status)
        //             {
        //                 var errorMessage = !resolveResponse.IsSuccessStatusCode ? await resolveResponse.Content.ReadAsStringAsync() : "Unable to resolve account number.";
        //                 return new BaseResponse
        //                 {
        //                     Succeeded = false,
        //                     Message = errorMessage,
        //                     StatusCode = HttpStatusCode.BadRequest
        //                 };
        //             }

        //             // Step 2: Create Transfer Recipient
        //             var recipientRequest = new
        //             {
        //                 type = "nuban",
        //                 name = request.AccountName,
        //                 account_number = request.AccountNo,
        //                 bank_code = request.BankCode,
        //                 currency = "NGN"
        //             };

        //             var recipientContent = new StringContent(JsonConvert.SerializeObject(recipientRequest), Encoding.UTF8, "application/json");
        //             var recipientResponse = await _httpClient.PostAsync($"{_config["Paystack:ApiBaseUri"]}/transferrecipient", recipientContent);
        //             var recipientResult = JsonConvert.DeserializeObject<dynamic>(await recipientResponse.Content.ReadAsStringAsync());

        //             if (!recipientResponse.IsSuccessStatusCode || !bool.Parse(recipientResult.status.ToString()))
        //             {
        //                 var errorMessage = !recipientResponse.IsSuccessStatusCode ? await recipientResponse.Content.ReadAsStringAsync() : "Unable to create transfer recipient.";
        //                 return new BaseResponse
        //                 {
        //                     Succeeded = false,
        //                     Message = errorMessage,
        //                     StatusCode = HttpStatusCode.BadRequest
        //                 };
        //             }

        //             // Step 3: Initiate Transfer
        //             var transferRequest = new
        //             {
        //                 source = "balance",
        //                 amount = request.Amount * 100, // Amount in kobo
        //                 recipient = recipientResult.data.recipient_code,
        //                 reason = "Withdrawal from wallet"
        //             };

        //             var transferContent = new StringContent(JsonConvert.SerializeObject(transferRequest), Encoding.UTF8, "application/json");
        //             var transferResponse = await _httpClient.PostAsync($"{_config["Paystack:ApiBaseUri"]}/transfer", transferContent);
        //             var transferResult = JsonConvert.DeserializeObject<dynamic>(await transferResponse.Content.ReadAsStringAsync());

        //             if (!transferResponse.IsSuccessStatusCode || !bool.Parse(transferResult.status.ToString()))
        //             {
        //                 var errorMessage = !transferResponse.IsSuccessStatusCode ? await transferResponse.Content.ReadAsStringAsync() : "Transfer failed.";
        //                 return new BaseResponse
        //                 {
        //                     Succeeded = false,
        //                     Message = errorMessage,
        //                     StatusCode = HttpStatusCode.InternalServerError
        //                 };
        //             }

        //             // Deduct the wallet balance after successful transfer
        //             await _userService.DeductFromWallet(userId, request.Amount);

        //             return new BaseResponse
        //             {
        //                 Succeeded = true,
        //                 Message = "Withdrawal successful",
        //                 StatusCode = HttpStatusCode.OK
        //             };
        //         }


    }
}