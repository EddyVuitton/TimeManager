﻿namespace TimeManager.Domain.Auth;

public class UserToken
{
    public int UserAccountId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}