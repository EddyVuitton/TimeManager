using Newtonsoft.Json;
using System.Text;
using TimeManager.Domain.Auth;
using TimeManager.Domain.Forms;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Account;

public class AccountService(HttpClient httpClient) : IAccountService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _ROUTE = "api/Account";

    public async Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm form)
    {
        var json = JsonConvert.SerializeObject(form);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ROUTE}/LoginAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<UserToken>>(responseContent);

        return deserialisedResponse ?? throw new NullReferenceException(nameof(UserToken));
    }
}