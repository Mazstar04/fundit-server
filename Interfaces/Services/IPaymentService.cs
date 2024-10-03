
using fundit_server.DTOs;
using fundit_server.Results;
using fundit_server.Wrapper;

namespace fundit_server.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<BaseResponse> MakePayment(MakePaymentRequest request);
        Task<PaginatedResult<GetPaymentResponse>> GetPaymentsByCampaignId(PaginationFilter paginationFilter, Guid campaignId);
        Task<PaginatedResult<GetPaymentResponse>> GetUserOutgoingPayments(PaginationFilter paginationFilter);
        Task<PaginatedResult<GetPaymentResponse>> GetUserIncomingPayments(PaginationFilter paginationFilter);
       
       
    }
}