using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fundit_server.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : BaseApiController
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [Route("token")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.Login(request);
        return Respond(result);
    }

     [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var result = await _authService.RegisterUser(request);
        return Respond(result);
    }

}
