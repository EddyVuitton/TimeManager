using Microsoft.EntityFrameworkCore;
using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Enums;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Repositories.Management;

public class Management(DBContext context) : IManagement
{
    private readonly DBContext _context = context;

    #region PublicMethods

    public async Task<List<ActivityDto>> GetActivitiesAsync(int userId)
    {
        var result = await _context.Activity
            .Where(x => x.ActivityList.UserId == userId)
            .Include(x => x.Repetition)
            .Select(s => new ActivityDto
            {
                ActivityId = s.Id,
                RepetitionId = s.RepetitionId,
                ActivityListId = s.ActivityListId,
                Day = s.Day,
                Title = s.Title,
                Description = s.Description ?? string.Empty,
                HourTypeId = s.HourTypeId,
                RepetitionTypeId = s.Id,
                IsOpen = false,
                UserId = userId
            }).ToListAsync();

        return result ?? [];
    }

    public async Task<List<ActivityDto>> AddActivityAsync(ActivityDto activity)
    {
        var repetitionTypeId = activity.RepetitionTypeId;
        var repetitionEnum = (RepetitionEnum)repetitionTypeId;
        var newActivities = new List<Activity>();
        var newRepetition = (await _context.Repetition.AddAsync(new Repetition()
        {
            RepetitionTypeId = activity.RepetitionTypeId,
            InitialTitle = activity.Title
        })).Entity;

        switch (repetitionEnum)
        {
            case RepetitionEnum.DoesntRepeat:
                newActivities = BasicHelper.AddActivityDoesntRepeat(activity, newRepetition);
                break;

            case RepetitionEnum.Daily:
                newActivities = BasicHelper.AddActivityDaily(activity, newRepetition);
                break;

            case RepetitionEnum.Monthly:
                newActivities = BasicHelper.AddActivityMonthly(activity, newRepetition);
                break;

            case RepetitionEnum.Weekly:
                newActivities = BasicHelper.AddActivityWeekly(activity, newRepetition);
                break;

            case RepetitionEnum.Annually:
                newActivities = BasicHelper.AddActivityAnnually(activity, newRepetition);
                break;

            default:
                break;
        }

        await _context.Activity.AddRangeAsync(newActivities);
        await _context.SaveChangesAsync();

        return BasicHelper.ToDtos(newActivities, repetitionTypeId);
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
        var result = await _context.ActivityList
            .Where(x => x.UserId == userId)
            .Select(s => new ActivityListDto
            {
                ID = s.Id,
                Name = s.Name,
                IsChecked = true,
                IsDefault = s.IsDefault
            }).ToListAsync();

        return result;
    }

    public async Task<ActivityDto> UpdateActivityAsync(ActivityDto activity)
    {
        var updatedActivity = await _context.Activity.FindAsync(activity.ActivityId);
        var repetition = await _context.Repetition.FirstOrDefaultAsync(x => x.Id == activity.RepetitionId);

        if (updatedActivity is not null && repetition is not null)
        {
            updatedActivity.Title = activity.Title;
            updatedActivity.Description = activity.Description;
            updatedActivity.ActivityListId = activity.ActivityListId;
            repetition.InitialTitle = activity.Title;

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
            UserAccount = user
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

    public async Task RemoveActivityListAsync(int activityListId)
    {
        var activityList = await _context.ActivityList.FirstOrDefaultAsync(x => x.Id == activityListId);

        if (activityList is not null && activityList.IsDefault)
        {
            throw new Exception("Listy domyślnej nie można usunąć");
        }

        var activities = _context.Activity
            .Where(x => x.ActivityList == activityList)
            .Include(x => x.Repetition);
        var repetitions = activities.Select(x => x.Repetition);

        _context.Activity.RemoveRange(activities);
        _context.Repetition.RemoveRange(repetitions);

        if (activityList is not null)
            _context.ActivityList.Remove(activityList);

        await _context.SaveChangesAsync();
    }

    public async Task MoveTaskToListAsync(int taskId, int taskListId)
    {
        var task = await _context.Activity.FirstOrDefaultAsync(x => x.Id == taskId);
        var taskList = await _context.ActivityList.FirstOrDefaultAsync(x => x.Id == taskListId);

        if (task is not null && taskList is not null)
        {
            task.ActivityList = taskList;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<RepetitionDto>> GetRepetitionsAsync(int userId)
    {
        var result = await _context.Activity
            .Where(x => x.ActivityList.UserId == userId)
            .Include(x => x.Repetition)
            .GroupBy(gb => new
            {
                gb.RepetitionId,
                gb.Repetition.RepetitionTypeId,
                RepetitionName = gb.Repetition.RepetitionType.Name,
                gb.Repetition.InitialTitle,
                gb.ActivityListId
            })
            .Select(s => new RepetitionDto
            {
                RepetitionId = s.Key.RepetitionId,
                RepetitionTypeId = s.Key.RepetitionTypeId,
                RepetitionName = s.Key.RepetitionName,
                InitialTitle = s.Key.InitialTitle,
                ActivityListId = s.Key.ActivityListId
            }).ToListAsync();

        return result ?? [];
    }

    public async Task RemoveRepetitionAsync(int repetitionId)
    {
        var repetitionToRemove = await _context.Repetition.FirstOrDefaultAsync(x => x.Id == repetitionId);
        var activitiesToRemove = _context.Activity.Where(a => a.RepetitionId == repetitionId);

        if (activitiesToRemove is not null && repetitionToRemove is not null)
        {
            _context.Repetition.Remove(repetitionToRemove);
            _context.Activity.RemoveRange(activitiesToRemove);

            await _context.SaveChangesAsync();
        }
    }

    public async Task MoveRepetitionToListAsync(int repetitionId, int taskListId)
    {
        var activityList = await _context.ActivityList.FirstOrDefaultAsync(x => x.Id == taskListId);
        var activities = _context.Activity.Where(a => a.RepetitionId == repetitionId);

        if (activityList is not null && activities is not null)
        {
            await activities.ForEachAsync(x => x.ActivityList = activityList);
            await _context.SaveChangesAsync();
        }
    }

    #endregion PublicMethods
}