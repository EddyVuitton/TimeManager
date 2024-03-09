using Newtonsoft.Json;
using System.Text;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Management;

public class ManagementService(HttpClient httpClient) : IManagementService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _ROUTE = "api/Management";

    public async Task<HttpResultT<List<ActivityDto>>> GetActivitiesAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_ROUTE}/GetActivitiesAsync?userId={userId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<ActivityDto>>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<ActivityDto>).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<ActivityDto>> AddActivityAsync(ActivityDto activity)
    {
        var json = JsonConvert.SerializeObject(activity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ROUTE}/AddActivityAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<ActivityDto>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(ActivityDto).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{_ROUTE}/GetRepetitionTypesAsync");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<RepetitionType>>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<RepetitionType>).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<List<HourType>>> GetHourTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{_ROUTE}/GetHourTypesAsync");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<HourType>>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<HourType>).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResult> RemoveActivityAsync(int activityId)
    {
        var response = await _httpClient.PostAsync($"{_ROUTE}/RemoveActivityAsync?activityId={activityId}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        return deserialisedResponse ?? throw new NullReferenceException(typeof(HttpResult).Name);
    }

    public async Task<HttpResultT<List<ActivityList>>> GetActivityListsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_ROUTE}/GetActivityListsAsync?userId={userId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<ActivityList>>>(responseContent);

        return deserialisedResponse ?? throw new NullReferenceException(typeof(List<ActivityList>).Name);
    }

    public async Task<HttpResultT<ActivityDto>> UpdateActivityAsync(ActivityDto activity)
    {
        var json = JsonConvert.SerializeObject(activity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_ROUTE}/UpdateActivityAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<ActivityDto>>(responseContent);

        return deserialisedResponse ?? throw new NullReferenceException(typeof(ActivityDto).Name);
    }
}