using Microsoft.EntityFrameworkCore;
using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Extensions;

namespace TimeManager.WebAPI.Repositories.Management;

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
        var newActivity = await context.Activity.AddAsync(new Activity()
        {
            Day = activity.Day,
            Title = activity.Title,
            Description = activity.Description,
            Task = activity.Task,
            HourTypeId = activity.HourTypeId,
            Repetition = newRepetitionEntity,
            UserId = activity.UserId,
            ActivityListId = activity.ActivityListId,
        });
        await context.SaveChangesAsync();
        activity.ActivityId = newActivity.Entity.Id;

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

    public async Task<List<ActivityList>> GetActivityListsAsync(int userId)
    {
        var result = await context.ActivityList.Where(x => x.UserId == userId).ToListAsync();

        return result ?? [];
    }

    public async Task<ActivityDto> UpdateActivityAsync(ActivityDto activity)
    {
        var updatedActivity = await context.Activity.FindAsync(activity.ActivityId);
        if (updatedActivity is not null)
        {
            updatedActivity.Title = activity.Title;
            await context.SaveChangesAsync();
        }

        return activity;
    }

    #endregion PublicMethods
}