using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TimeManager.Domain.Auth;
using TimeManager.WebUI.Extensions;

namespace TimeManager.WebUI.Auth;

public class JWTAuthenticationStateProvider(IJSRuntime js, HttpClient httpClient) : AuthenticationStateProvider, ILoginService
{
    //https://www.udemy.com/course/programming-in-blazor-aspnet-core/learn/lecture/17136788#overview
    private readonly IJSRuntime _js = js;

    private readonly HttpClient _httpClient = httpClient;
    private const string _TOKENKEY = "TOKENKEY";
    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    #region PublicMethods

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _js.GetFromLocalStorage(_TOKENKEY);

            if (string.IsNullOrEmpty(token))
                return _anonymous;

            return BuildAuthenticationState(token);
        }
        catch
        {
            return await Task.FromResult(_anonymous);
        }
    }

    public AuthenticationState BuildAuthenticationState(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
    }

    public async Task<string?> IsLoggedInAsync()
    {
        var authenticationState = await this.GetAuthenticationStateAsync();

        if (authenticationState is not null && authenticationState.User.Claims.Any())
        {
            var result = authenticationState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return result;
        }

        return null;
    }

    public async Task LogoutIfExpiredTokenAsync()
    {
        var now = DateTime.Now;
        var validTo = await TokenValidToAsync();

        if (validTo.CompareTo(now) <= 0)
        {
            await LogoutAsync();
        }
    }

    public async Task LoginAsync(UserToken userToken)
    {
        await _js.SetInLocalStorage(_TOKENKEY, userToken.Token);
        var authState = BuildAuthenticationState(userToken.Token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task LogoutAsync()
    {
        await CleanUpAsync();
    }

    #endregion PublicMethods

    #region PrivateMethods

    private static List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        var roles = new object();

        keyValuePairs?.TryGetValue(ClaimTypes.Role, out roles);
        var rolesAsString = roles?.ToString();

        if (!string.IsNullOrEmpty(rolesAsString))
        {
            if (rolesAsString.Trim().StartsWith('['))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(rolesAsString);

                if (parsedRoles is null)
                    return [];

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, rolesAsString));
            }

            keyValuePairs?.Remove(ClaimTypes.Role);
        }

        if (keyValuePairs is not null)
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private async Task CleanUpAsync()
    {
        await _js.RemoveItemFromLocalStorage(_TOKENKEY);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private async Task<DateTime> TokenValidToAsync()
    {
        var token = await _js.GetFromLocalStorage(_TOKENKEY);

        if (!string.IsNullOrEmpty(token))
        {
            var validTo = new JwtSecurityTokenHandler().ReadToken(token).ValidTo.ToLocalTime();

            return validTo;
        }

        return DateTime.Now;
    }

    #endregion PrivateMethods
}