using fundit_server.DTOs;
using fundit_server.Results;

namespace fundit_server.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> FundWallet(Guid userId, decimal amount);
        Task<bool> DeductFromWallet(Guid userId, decimal amount);
        Task<BaseResponse<GetUserStatsResponse>> GetUserStats();

    }
}