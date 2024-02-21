using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Http;

namespace TimeManager.WebAPI.APIs.Management.Interfaces;

public interface IManagementService
{
    Task<HttpResultT<ActivityDto>> AddActivityAsync(ActivityDto activity);
    Task<HttpResultT<List<HourType>>> GetHourTypesAsync();
    Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync();
    Task<HttpResultT<List<ActivityDto>>> GetActivitiesAsync(int userId);
    Task<HttpResult> RemoveActivityAsync(int activityId);
}