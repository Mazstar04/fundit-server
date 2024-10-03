using System.Net;

namespace fundit_server.Results;

public interface IActionResult
{
    HttpStatusCode Status { get; set; }
    
    string ErrorMessage { get; set; }
    
}