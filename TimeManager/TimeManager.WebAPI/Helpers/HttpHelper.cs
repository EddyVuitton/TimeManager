using System.Net;
using TimeManager.Domain.Http;

namespace TimeManager.WebAPI.Helpers;

public class HttpHelper
{
    public static HttpResultT<T> Error<T>(Exception e)
    {
        var HttpResult = new HttpResultT<T>()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = e.Message,
            IsSuccess = false
        };

        return HttpResult;
    }

    public static HttpResultT<T> Ok<T>(T data, string message = "")
    {
        var HttpResult = new HttpResultT<T>()
        {
            Data = data,
            StatusCode = HttpStatusCode.OK,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            IsSuccess = true
        };
        return HttpResult;
    }

    public static HttpResult Error(Exception ex)
    {
        var HttpResult = new HttpResult()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = ex.Message,
            IsSuccess = false
        };

        return HttpResult;
    }

    public static HttpResult Ok(string message = "")
    {
        var HttpResult = new HttpResult()
        {
            StatusCode = HttpStatusCode.OK,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            IsSuccess = true
        };

        return HttpResult;
    }
}