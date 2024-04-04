using Newtonsoft.Json;
using System.Text;
using TimeManager.Domain.Auth;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Forms;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Account;

public class AccountService(HttpClient httpClient) : IAccountService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _route = "api/Account";

    public async Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm form)
    {
        var json = JsonConvert.SerializeObject(form);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_route}/LoginAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<UserToken>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResult> RegisterAsync(RegisterAccountForm form)
    {
        var json = JsonConvert.SerializeObject(form);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_route}/RegisterAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<UserAccount>> GetUserByEmailAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{_route}/GetUserByEmailAsync?email={email}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<UserAccount>>(responseContent);

        return deserialisedResponse ?? new();
    }
}