using System.Net;

namespace TimeManager.WebAPI.Http;

public class HttpResult
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSucces { get; set; }
}