using TimeManager.Domain.DTOs;

namespace TimeManager.WebAPI.APIs.Management;

public interface IManagementContext
{
    Task<List<ActivityDto>> GetUserActivitiesAsync(int userId);
}