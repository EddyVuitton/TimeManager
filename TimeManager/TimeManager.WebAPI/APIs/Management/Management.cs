using Microsoft.EntityFrameworkCore;
using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.APIs.Management.Interfaces;
using TimeManager.WebAPI.Extensions;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.APIs.Management;

public class Management(DBContext context) : IManagement
{
    #region PublicMethods

    public async Task<List<ActivityDto>> GetActivitiesAsync(int userId)
    {
        var hT = new object[]
        {
            context.CreateParameter("userId", userId, SqlDbType.Int)
        };
        var result = await context.SqlQueryAsync<ActivityDto>("exec p_get_user_activities @userId;", hT);

        return result ?? [];
    }

    public async Task<ActivityDto> AddActivityAsync(ActivityDto activity)
    {
        var newRepetitionEntity = (await context.Repetition.AddAsync(new Repetition()
        {
            Day = DateTime.Now,
            RepetitionTypeId = activity.RepetitionTypeId
        })).Entity;
        await context.Activity.AddAsync(new Activity()
        {
            Day = activity.Day,
            Description = activity.Description,
            Task = activity.Task,
            HourTypeId = activity.HourTypeId,
            Repetition = newRepetitionEntity,
            UserId = activity.UserId
        });
        await context.SaveChangesAsync();

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

    public async Task RemoveActivityAsync(int activityId)
    {
        var activityToRemove = await context.Activity.Include(a => a.Repetition).FirstOrDefaultAsync(a => a.Id == activityId);
        if (activityToRemove is not null)
        {
            context.Repetition.Remove(activityToRemove.Repetition);
            context.Activity.Remove(activityToRemove);

            await context.SaveChangesAsync();
        }
    }

    #endregion PublicMethods
}