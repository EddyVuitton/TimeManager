using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Extensions;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.APIs.Management;

public class Management(DBContext context) : IManagementContext
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
            Hour = activity.Hour,
            Repetition = repetition,
            UserId = activity.UserId
        });

        return activity;
    }

    #endregion PublicMethods
}