using Newtonsoft.Json;
using System.Text;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Management;

public class ManagementService(HttpClient httpClient) : IManagementService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _route = "api/Management";

    public async Task<HttpResultT<List<ActivityDto>>> GetActivitiesAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_route}/GetActivitiesAsync?userId={userId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<ActivityDto>>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<List<ActivityDto>>> AddActivityAsync(ActivityDto activity)
    {
        var json = JsonConvert.SerializeObject(activity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_route}/AddActivityAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<ActivityDto>>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{_route}/GetRepetitionTypesAsync");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<RepetitionType>>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<List<HourType>>> GetHourTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{_route}/GetHourTypesAsync");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<HourType>>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResult> RemoveActivityAsync(int activityId)
    {
        var response = await _httpClient.PostAsync($"{_route}/RemoveActivityAsync?activityId={activityId}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<ActivityDto>> UpdateActivityAsync(ActivityDto activity)
    {
        var json = JsonConvert.SerializeObject(activity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_route}/UpdateActivityAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<ActivityDto>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<List<ActivityListDto>>> GetActivityListsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_route}/GetActivityListsAsync?userId={userId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<ActivityListDto>>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<ActivityListDto>> AddActivityListAsync(ActivityListDto activityList)
    {
        var json = JsonConvert.SerializeObject(activityList);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_route}/AddActivityListAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<ActivityListDto>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResultT<ActivityListDto>> UpdateActivityListAsync(ActivityListDto activityList)
    {
        var json = JsonConvert.SerializeObject(activityList);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_route}/UpdateActivityListAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<ActivityListDto>>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResult> RemoveActivityListAsync(int activityListId)
    {
        var response = await _httpClient.PostAsync($"{_route}/RemoveActivityListAsync?activityListId={activityListId}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<HttpResult> MoveTaskToList(int taskId, int taskListId)
    {
        var response = await _httpClient.PostAsync($"{_route}/MoveTaskToListAsync?taskId={taskId}&taskListId={taskListId}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        return deserialisedResponse ?? new();
    }
}