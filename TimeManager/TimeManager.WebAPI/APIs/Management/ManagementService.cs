using Newtonsoft.Json;
using TimeManager.Domain.DTOs;
using TimeManager.WebAPI.Http;

namespace TimeManager.WebAPI.APIs.Management;

public class ManagementService(HttpClient httpClient) : IManagementService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _ROUTE = "api/Management";

    public async Task<HttpResultT<List<ActivityDto>>> GetUserActivitiesAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_ROUTE}/GetUserActivitiesAsync?userId={userId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<ActivityDto>>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<ActivityDto>).Name);

        return deserialisedResponse;
    }
}