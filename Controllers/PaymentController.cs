using fundit_server.DTOs;
using fundit_server.Enums;
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

    [Route("initialize-payment")]
    [HttpPost]
    public async Task<IActionResult> InitializePayment([FromBody] InitializePaymentRequest request)
    {
        var result = await _paymentService.InitializePayment(request);
        return Respond(result);
    }

    [Route("verify-payment")]
    [HttpPost]
    public async Task<IActionResult> PaymentCallback(string reference)
    {
        var result = await _paymentService.PaymentCallback(reference);
        return Respond(result);
    }


    [Route("get-user-payments")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserOutgoingPayments([FromQuery] PaginationFilter paginationFilter,[FromQuery] PaymentStatus? status)
    {
        var result = await _paymentService.GetUserPayments(paginationFilter, status);
        return Ok(result);
    }

    [Route("get-payments-by-campaignid")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPaymentsByCampaignId([FromQuery] PaginationFilter paginationFilter,[FromQuery] PaymentStatus? status, [FromQuery] Guid campaignId)
    {
        var result = await _paymentService.GetPaymentsByCampaignId(paginationFilter,status, campaignId);
        return Ok(result);
    }


}
