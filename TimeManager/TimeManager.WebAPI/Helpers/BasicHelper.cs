using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;

namespace TimeManager.WebAPI.Helpers;

public static class BasicHelper
{
    public static List<Activity> AddActivityDoesntRepeat(ActivityDto activity, Repetition repetition)
    {
        var result = new List<Activity>();

        var newActivity = new Activity()
        {
            Day = activity.Day,
            Title = activity.Title,
            Description = activity.Description,
            HourTypeId = activity.HourTypeId,
            Repetition = repetition,
            UserId = activity.UserId,
            ActivityListId = activity.ActivityListId
        };

        result.Add(newActivity);

        return result;
    }

    public static List<Activity> AddActivityDaily(ActivityDto activity, Repetition repetition)
    {
        var start = activity.Day;
        var end = start.AddMonths(1).AddDays(8);
        var result = new List<Activity>();

        while (!start.Date.Equals(end.Date))
        {
            result.Add(new Activity()
            {
                Day = start,
                Title = activity.Title,
                Description = activity.Description,
                HourTypeId = activity.HourTypeId,
                Repetition = repetition,
                UserId = activity.UserId,
                ActivityListId = activity.ActivityListId
            });

            start = start.AddDays(1);
        }

        return result;
    }

    public static List<Activity> AddActivityWeekly(ActivityDto activity, Repetition repetition)
    {
        var start = activity.Day;
        var end = start.AddDays(7 * 14);
        var result = new List<Activity>();

        while (!start.Date.Equals(end.Date))
        {
            result.Add(new Activity()
            {
                Day = start,
                Title = activity.Title,
                Description = activity.Description,
                HourTypeId = activity.HourTypeId,
                Repetition = repetition,
                UserId = activity.UserId,
                ActivityListId = activity.ActivityListId
            });

            start = start.AddDays(7);
        }

        return result;
    }

    public static List<Activity> AddActivityMonthly(ActivityDto activity, Repetition repetition)
    {
        var start = activity.Day;
        var end = start.AddMonths(12);
        var result = new List<Activity>();

        while (!start.Date.Equals(end.Date))
        {
            result.Add(new Activity()
            {
                Day = start,
                Title = activity.Title,
                Description = activity.Description,
                HourTypeId = activity.HourTypeId,
                Repetition = repetition,
                UserId = activity.UserId,
                ActivityListId = activity.ActivityListId
            });

            start = start.AddMonths(1);
        }

        return result;
    }

    public static List<Activity> AddActivityAnnually(ActivityDto activity, Repetition repetition)
    {
        var start = activity.Day;
        var end = start.AddYears(10);
        var result = new List<Activity>();

        while (!start.Date.Equals(end.Date))
        {
            result.Add(new Activity()
            {
                Day = start,
                Title = activity.Title,
                Description = activity.Description,
                HourTypeId = activity.HourTypeId,
                Repetition = repetition,
                UserId = activity.UserId,
                ActivityListId = activity.ActivityListId
            });

            start = start.AddYears(1);
        }

        return result;
    }

    public static List<ActivityDto> ToDtos(List<Activity> activities, int repetitionTypeId)
    {
        var result = new List<ActivityDto>();
        
        foreach (var activity in activities)
        {
            result.Add(new ActivityDto()
            {
                ActivityListId = activity.ActivityListId,

                Day = activity.Day,
                Title = activity.Title,
                Description = activity.Description ?? string.Empty,
                HourTypeId = activity.HourTypeId,
                RepetitionTypeId = repetitionTypeId,
                UserId = activity.UserId               
            });
        }

        return result;
    }
}