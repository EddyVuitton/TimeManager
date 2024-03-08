using Newtonsoft.Json;
using TimeManager.Domain.Auth;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.AccontService;

public class AccountService(HttpClient httpClient) : IAccountService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _ROUTE = "api/Account";

    public async Task<HttpResultT<UserToken>> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsync($"{_ROUTE}/LoginAsync?email={email}&password={password}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<UserToken>>(responseContent);

        return deserialisedResponse ?? throw new NullReferenceException(nameof(UserToken));
    }
}