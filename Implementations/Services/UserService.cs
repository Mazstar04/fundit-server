

using System.Net;
using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;
using fundit_server.Interfaces.Services;
using fundit_server.Results;
using Microsoft.EntityFrameworkCore;

namespace fundit_server.Implementations.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepo;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IUserIdentityService userIdentityService, IMapper mapper)
        {
            _userRepo = userRepo;
            _userIdentityService = userIdentityService;
            _mapper = mapper;
        }


        public async Task<bool> FundWallet(Guid userId, decimal amount)
        {
            var user = await _userRepo.GetAsync(userId);
            if (user != null)
            {
                user.WalletBalance += amount;
                await _userRepo.UpdateAsync(user);
                await _userRepo.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<bool> DecuctFromWallet(Guid userId, decimal amount)
        {
            var user = await _userRepo.GetAsync(userId);
            if (user != null)
            {
                user.WalletBalance -= amount;
                await _userRepo.UpdateAsync(user);
                await _userRepo.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<BaseResponse<GetUserStatsResponse>> GetUserStats()
        {
            var userId = _userIdentityService.GetUserId();
            var query = _userRepo.Query();

            var user = await query
            .Where(u => u.Id == userId)
            .Include(q => q.Payments)
            .Include(q => q.Withdrawals)
            .Include(q => q.Campaigns)
            .ThenInclude(q => q.Payments)
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return new BaseResponse<GetUserStatsResponse>
                {
                    Succeeded = false,
                    Message = "user not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }
            var stats = new GetUserStatsResponse
            {
                WalletBalance = user.WalletBalance,
                TotalAmountDonated = user.Payments.Sum(s => s.Amount),
                TotalAmountWithdrawn = user.Withdrawals.Sum(s => s.Amount),
                TotalAmountReceived = user.Campaigns.Sum(p => p.Payments.Sum(pay => pay.Amount)),
                TotalActiveCampaigns = user.Campaigns.Where(s => s.Payments.Sum(p => p.Amount) < s.Amount).Count(),
                TotalInactiveCampaigns = user.Campaigns.Where(s => s.Payments.Sum(p => p.Amount) >= s.Amount).Count(),
            };


            return new BaseResponse<GetUserStatsResponse>
            {
                Data = stats,
                Succeeded = true,
                Message = "Stats fetched successful",
                StatusCode = HttpStatusCode.OK,
            };
        }
    }
}