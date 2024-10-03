using System.Net;

namespace fundit_server.Results;

public class BaseResponse
{
    public string Message { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
    public bool Succeeded { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
public class BaseResponse<T> : BaseResponse
{

    public T Data { get; set; }
}
