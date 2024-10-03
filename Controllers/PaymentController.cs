using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using fundit_server.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fundit_server.Controllers;

[ApiController]
[Route("api/v1/payment")]
public class PaymentController : BaseApiController
{

    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Route("make-payment")]
    [HttpPost]
    public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest request)
    {
        var result = await _paymentService.MakePayment(request);
        return Respond(result);
    }


    [Route("get-incoming-payments")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserIncomingPayments([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await _paymentService.GetUserIncomingPayments(paginationFilter);
        return Ok(result);
    }

    [Route("get-outgoing-payments")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserOutgoingPayments([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await _paymentService.GetUserOutgoingPayments(paginationFilter);
        return Ok(result);
    }

    [Route("get-payments-by-campaignid")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPaymentsByCampaignId([FromQuery] PaginationFilter paginationFilter, [FromQuery]  Guid campaignId)
    {
        var result = await _paymentService.GetPaymentsByCampaignId(paginationFilter, campaignId);
        return Ok(result);
    }


}
