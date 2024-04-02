using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Management;

public interface IManagementService
{
    Task<HttpResultT<ActivityDto>> AddActivityAsync(ActivityDto activity);
    Task<HttpResultT<List<HourType>>> GetHourTypesAsync();
    Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync();
    Task<HttpResultT<List<ActivityDto>>> GetActivitiesAsync(int userId);
    Task<HttpResult> RemoveActivityAsync(int activityId);
    Task<HttpResultT<ActivityDto>> UpdateActivityAsync(ActivityDto activity);
    Task<HttpResultT<List<ActivityListDto>>> GetActivityListsDtoAsync(int userId);
}