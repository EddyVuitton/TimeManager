using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Http;

namespace TimeManager.WebAPI.APIs.Management;

public interface IManagementService
{
    Task<HttpResultT<ActivityDto>> AddUserActivityAsync(ActivityDto activity);
    Task<HttpResultT<List<HourType>>> GetHourTypesAsync();
    Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync();
    Task<HttpResultT<List<ActivityDto>>> GetUserActivitiesAsync(int userId);
}