using TimeManager.Domain.DTOs;
using TimeManager.WebAPI.Http;

namespace TimeManager.WebAPI.APIs.Management;

public interface IManagementService
{
    Task<HttpResultT<List<ActivityDto>>> GetUserActivitiesAsync(int userId);
}