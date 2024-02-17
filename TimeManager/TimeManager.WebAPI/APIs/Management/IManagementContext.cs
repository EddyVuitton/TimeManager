using TimeManager.Domain.DTOs;

namespace TimeManager.WebAPI.APIs.Management;

public interface IManagementContext
{
    Task<ActivityDto> AddUserActivityAsync(ActivityDto activity);
    Task<List<ActivityDto>> GetUserActivitiesAsync(int userId);
}