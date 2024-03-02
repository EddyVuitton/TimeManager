﻿using System.Net;

namespace TimeManager.Domain.Http;

public class HttpResultT<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public T Data { get; set; } = default!;
}