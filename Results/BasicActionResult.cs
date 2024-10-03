using System.Net;

namespace fundit_server.Results;

public class BasicActionResult : IActionResult
{
    public HttpStatusCode Status { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public BasicActionResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        Status = HttpStatusCode.BadRequest;
    }

    public BasicActionResult()
    {
        Status = HttpStatusCode.OK;
    }
    
    public BasicActionResult(HttpStatusCode status)
    {
        Status = status;
    }
}