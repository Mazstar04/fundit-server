using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using fundit_server.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fundit_server.Controllers;

[ApiController]
[Route("api/v1/withdraw")]
public class WithdrawalController : BaseApiController
{

    private readonly IWithdrawalService _withdrawalService;

    public WithdrawalController(IWithdrawalService withdrawalService)
    {
        _withdrawalService = withdrawalService;
    }

    [Route("withdraw")]
    [HttpPost]
    public async Task<IActionResult> WithdrawMoney([FromBody] WithdrawMoneyRequest request)
    {
        var result = await _withdrawalService.WithdrawMoney(request);
        return Respond(result);
    }


    [Route("get-user-withdrawals")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserWithdrawals([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await _withdrawalService.GetUserWithdrawals(paginationFilter);
        return Ok(result);
    }

    
}
