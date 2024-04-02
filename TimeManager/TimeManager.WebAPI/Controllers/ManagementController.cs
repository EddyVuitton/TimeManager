using Microsoft.AspNetCore.Mvc;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Http;
using TimeManager.WebAPI.Helpers;
using TimeManager.WebAPI.Repositories.Management;

namespace TimeManager.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagementController(IManagement businessLogic) : ControllerBase
{
    private readonly IManagement _businessLogic = businessLogic;

    #region Gets

    [HttpGet("GetActivitiesAsync")]
    public async Task<HttpResultT<List<ActivityDto>>> GetActivitiesAsync(int userId)
    {
        try
        {
            var result = await _businessLogic.GetActivitiesAsync(userId);
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

    [HttpGet("GetHourTypesAsync")]
    public async Task<HttpResultT<List<HourType>>> GetHourTypesAsync()
    {
        try
        {
            var result = await _businessLogic.GetHourTypesAsync();
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<List<HourType>>(e);
        }
    }

    [HttpGet("GetActivityListsAsync")]
    public async Task<HttpResultT<List<ActivityListDto>>> GetActivityListsAsync(int userId)
    {
        try
        {
            var result = await _businessLogic.GetActivityListsAsync(userId);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<List<ActivityListDto>>(e);
        }
    }

    #endregion Gets

    #region Posts

    [HttpPost("AddActivityAsync")]
    public async Task<HttpResultT<ActivityDto>> AddActivityAsync(ActivityDto activity)
    {
        try
        {
            var result = await _businessLogic.AddActivityAsync(activity);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<ActivityDto>(e);
        }
    }

    [HttpPost("RemoveActivityAsync")]
    public async Task<HttpResult> RemoveActivityAsync(int activityId)
    {
        try
        {
            await _businessLogic.RemoveActivityAsync(activityId);
            return HttpHelper.Ok();
        }
        catch (Exception e)
        {
            return HttpHelper.Error(e);
        }
    }

    [HttpPost("AddActivityListAsync")]
    public async Task<HttpResultT<ActivityListDto>> AddActivityListAsync(ActivityListDto activityList)
    {
        try
        {
            var result = await _businessLogic.AddActivityListAsync(activityList);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<ActivityListDto>(e);
        }
    }

    #endregion Posts

    #region Puts

    [HttpPut("UpdateActivityAsync")]
    public async Task<HttpResultT<ActivityDto>> UpdateActivityAsync(ActivityDto activity)
    {
        try
        {
            var result = await _businessLogic.UpdateActivityAsync(activity);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<ActivityDto>(e);
        }
    }

    #endregion Puts
}