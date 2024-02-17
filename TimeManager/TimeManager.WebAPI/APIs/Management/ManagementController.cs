using Microsoft.AspNetCore.Mvc;
using TimeManager.Domain.DTOs;
using TimeManager.WebAPI.Helpers;
using TimeManager.WebAPI.Http;

namespace TimeManager.WebAPI.APIs.Management;

[ApiController]
[Route("api/[controller]")]
public class ManagementController(IManagementContext businessLogic) : ControllerBase
{
    private readonly IManagementContext _businessLogic = businessLogic;

    [HttpGet("GetUserActivitiesAsync")]
    public async Task<HttpResultT<List<ActivityDto>>> GetUserActivitiesAsync(int userId)
    {
        try
        {
            var result = await _businessLogic.GetUserActivitiesAsync(userId);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<List<ActivityDto>>(e);
        }
    }

    [HttpPost("AddUserActivityAsync")]
    public async Task<HttpResultT<ActivityDto>> AddUserActivityAsync(ActivityDto activity)
    {
        try
        {
            var result = await _businessLogic.AddUserActivityAsync(activity);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<ActivityDto>(e);
        }
    }
}