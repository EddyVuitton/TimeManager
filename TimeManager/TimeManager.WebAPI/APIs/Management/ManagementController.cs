using Microsoft.AspNetCore.Mvc;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Helpers;
using TimeManager.WebAPI.Http;

namespace TimeManager.WebAPI.APIs.Management;

[ApiController]
[Route("api/[controller]")]
public class ManagementController(IManagement businessLogic) : ControllerBase
{
    private readonly IManagement _businessLogic = businessLogic;

    #region Gets

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

    [HttpGet("GetRepetitionTypesAsync")]
    public async Task<HttpResultT<List<RepetitionType>>> GetRepetitionTypesAsync()
    {
        try
        {
            var result = await _businessLogic.GetRepetitionTypesAsync();
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<List<RepetitionType>>(e);
        }
    }

    #endregion Gets

    #region Posts

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

    #endregion Posts
}