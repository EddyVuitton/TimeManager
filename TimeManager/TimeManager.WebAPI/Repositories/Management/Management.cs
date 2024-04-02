using Microsoft.EntityFrameworkCore;
using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.Extensions;

namespace TimeManager.WebAPI.Repositories.Management;

public class Management(DBContext context) : IManagement
{
    private readonly DBContext _context = context;

    #region PublicMethods

    public async Task<List<ActivityDto>> GetActivitiesAsync(int userId)
    {
        var hT = new object[]
        {
            _context.CreateParameter("userId", userId, SqlDbType.Int)
        };
        var result = await _context.SqlQueryAsync<ActivityDto>("exec p_get_user_activities @userId;", hT);

        return result ?? [];
    }

    public async Task<ActivityDto> AddActivityAsync(ActivityDto activity)
    {
        var newRepetitionEntity = (await _context.Repetition.AddAsync(new Repetition()
        {
            Day = DateTime.Now,
            RepetitionTypeId = activity.RepetitionTypeId
        })).Entity;
        var newActivity = await _context.Activity.AddAsync(new Activity()
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
        await _context.SaveChangesAsync();
        activity.ActivityId = newActivity.Entity.Id;

        return activity;
    }

    public async Task<List<RepetitionType>> GetRepetitionTypesAsync()
    {
        var result = await _context.RepetitionType.ToListAsync();

        return result ?? [];
    }

    public async Task<List<HourType>> GetHourTypesAsync()
    {
        var result = await _context.HourType.ToListAsync();

        return result ?? [];
    }

    public async Task RemoveActivityAsync(int activityId)
    {
        var activityToRemove = await _context.Activity.Include(a => a.Repetition).FirstOrDefaultAsync(a => a.Id == activityId);
        if (activityToRemove is not null)
        {
            _context.Repetition.Remove(activityToRemove.Repetition);
            _context.Activity.Remove(activityToRemove);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<ActivityListDto>> GetActivityListsAsync(int userId)
    {
        var activityLists = await _context.ActivityList.Where(x => x.UserId == userId).ToListAsync();
        var listsDto =
            from aL in activityLists
            select new ActivityListDto
            {
                ID = aL.Id,
                Name = aL.Name,
                IsChecked = true
            };

        return listsDto.ToList();
    }

    public async Task<ActivityDto> UpdateActivityAsync(ActivityDto activity)
    {
        var updatedActivity = await _context.Activity.FindAsync(activity.ActivityId);
        if (updatedActivity is not null)
        {
            updatedActivity.Title = activity.Title;
            updatedActivity.Description = activity.Description;
            await _context.SaveChangesAsync();
        }

        return activity;
    }

    public async Task<ActivityListDto> AddActivityListAsync(ActivityListDto activityList)
    {
        var user = await _context.UserAccount.FirstAsync(x => x.Id == activityList.UserId);
        var newActivityList = await _context.ActivityList.AddAsync(new ActivityList()
        {
            Name = activityList.Name,
            IsDefault = false,
            User = user
        });
        await _context.SaveChangesAsync();
        activityList.ID = newActivityList.Entity.Id;

        return activityList;
    }

    public async Task<ActivityListDto> UpdateActivityListAsync(ActivityListDto activityList)
    {
        var updatedActivityList = await _context.ActivityList.FindAsync(activityList.ID);
        if (updatedActivityList is not null)
        {
            updatedActivityList.Name = activityList.Name;
            await _context.SaveChangesAsync();
        }

        return activityList;
    }

    #endregion PublicMethods
}