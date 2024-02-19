using Microsoft.EntityFrameworkCore;
using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Extensions;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.APIs.Management;

public class Management(DBContext context) : IManagement
{
    #region PublicMethods

    public async Task<List<ActivityDto>> GetUserActivitiesAsync(int userId)
    {
        var hT = new object[]
        {
            context.CreateParameter("userId", userId, SqlDbType.Int)
        };
        var result = await context.SqlQueryAsync<ActivityDto>("exec p_get_user_activities @userId;", hT);

        return result ?? [];
    }

    public async Task<ActivityDto> AddUserActivityAsync(ActivityDto activity)
    {
        var repetition = await ManagementHelper.AddRepetitionAsync(context, new Repetition()
        {
            Day = DateTime.Now,
            RepetitionTypeId = activity.RepetitionTypeId
        });
        await ManagementHelper.AddActivityAsync(context, new Activity()
        {
            Day = activity.Day,
            Description = activity.Description,
            Task = activity.Task,
            HourTypeId = activity.HourTypeId,
            Repetition = repetition,
            UserId = activity.UserId
        });

        return activity;
    }

    public async Task<List<RepetitionType>> GetRepetitionTypesAsync()
    {
        var result = await context.RepetitionType.ToListAsync();

        return result ?? [];
    }

    public async Task<List<HourType>> GetHourTypesAsync()
    {
        var result = await context.HourType.ToListAsync();

        return result ?? [];
    }

    #endregion PublicMethods
}