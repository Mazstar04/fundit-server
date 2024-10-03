using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fundit_server.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : BaseApiController
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Route("get-stats")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserStats()
    {
        var result = await _userService.GetUserStats();
        return Respond(result);

    }

}
