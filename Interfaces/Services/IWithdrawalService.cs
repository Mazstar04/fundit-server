
using fundit_server.DTOs;
using fundit_server.Results;
using fundit_server.Wrapper;

namespace fundit_server.Interfaces.Services
{
    public interface IWithdrawalService
    {
        Task<BaseResponse> WithdrawMoney(WithdrawMoneyRequest request);
        Task<PaginatedResult<GetWithdrawalResponse>> GetUserWithdrawals(PaginationFilter paginationFilter);
        
       
    }
}