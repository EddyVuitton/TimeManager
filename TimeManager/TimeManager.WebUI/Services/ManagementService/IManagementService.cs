using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.ManagementService;

public interface IManagementService
{
    Task<HttpResultT<ActivityDto>> AddActivityAsync(ActivityDto activity);
    Task<HttpResultT<List<HourType>>> GetHourTypesAsync();
    Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync();
    Task<HttpResultT<List<ActivityDto>>> GetActivitiesAsync(int userId);
    Task<HttpResult> RemoveActivityAsync(int activityId);
    Task<HttpResultT<List<ActivityList>>> GetActivityListsAsync(int userId);
    Task<HttpResultT<ActivityDto>> UpdateActivityAsync(ActivityDto activity);
}