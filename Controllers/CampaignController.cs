using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using fundit_server.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fundit_server.Controllers;

[ApiController]
[Route("api/v1/campaign")]
public class CampaignController : BaseApiController
{

    private readonly ICampaignService _campaignService;

    public CampaignController(ICampaignService campaignService)
    {
        _campaignService = campaignService;
    }

    [Route("add-campaign")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddCampaign([FromBody] CreateCampaignRequest request)
    {
        var result = await _campaignService.CreateCampaign(request);
        return Respond(result);
    }

    [Route("get-all-campaigns")]
    [HttpGet]
    public async Task<IActionResult> GetAllCampaigns([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await _campaignService.GetAllCampaigns(paginationFilter);
        return Ok(result);
    }

    [Route("get-user-campaigns")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserCampaigns([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await _campaignService.GetUserCampaigns(paginationFilter);
        return Ok(result);
    }

    [Route("get-campaigns-by-userid")]
    [HttpGet]
    public async Task<IActionResult> GetCampaignsByUserId([FromQuery] PaginationFilter paginationFilter, [FromQuery]  Guid userId)
    {
        var result = await _campaignService.GetCampaignsByUserId(paginationFilter, userId);
        return Ok(result);
    }

    [Route("get-campaign-detail")]
    [HttpGet]
    public async Task<IActionResult> GetCampaignDetail(Guid campaignId)
    {
        var result = await _campaignService.GetCampaignDetail(campaignId);
        return Respond(result);
    }

}
