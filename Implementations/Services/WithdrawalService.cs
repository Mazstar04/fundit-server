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
    public class WithdrawalService : IWithdrawalService
    {
        private readonly IWithdrawalRepository _withdrawalRepo;
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public WithdrawalService(IMapper mapper, IUserIdentityService userIdentityService, IUserService userService, IWithdrawalRepository withdrawalRepo, IUserRepository userRepo)
        {
            _mapper = mapper;
            _userIdentityService = userIdentityService;
            _userService = userService;
            _withdrawalRepo = withdrawalRepo;
            _userRepo = userRepo;
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
            await _userService.DecuctFromWallet(userId, request.Amount);

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



    }
}