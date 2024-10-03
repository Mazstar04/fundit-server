using System.Net;
using fundit_server.Results;
using Microsoft.AspNetCore.Mvc;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace fundit_server.Controllers;

public class BaseApiController : ControllerBase
{


    [NonAction]
    public IActionResult Respond(BaseResponse result)
    {

        if (result.StatusCode == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        
        return !string.IsNullOrEmpty(result.Message) ? StatusCode((int)result.StatusCode, result.Message) : StatusCode((int)result.StatusCode, result);
    }
    
}