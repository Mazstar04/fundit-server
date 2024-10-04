
using fundit_server.DTOs;
using fundit_server.Enums;
using fundit_server.Results;
using fundit_server.Wrapper;

namespace fundit_server.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<BaseResponse<string>> InitializePayment(InitializePaymentRequest request);
        Task<PaginatedResult<GetPaymentResponse>> GetPaymentsByCampaignId(PaginationFilter paginationFilter, PaymentStatus? status, Guid campaignId);
        Task<PaginatedResult<GetPaymentResponse>> GetUserPayments(PaginationFilter paginationFilter, PaymentStatus? status);
        Task<BaseResponse> PaymentCallback(string reference);
    }
}