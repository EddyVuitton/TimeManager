using TimeManager.Domain.Context;
using TimeManager.Domain.Entities;

namespace TimeManager.WebAPI.Helpers;

public static class ManagementHelper
{
    public static async Task<Repetition> AddRepetitionAsync(DBContext context, Repetition repetition)
    {
        await context.Repetition.AddAsync(repetition);
        await context.SaveChangesAsync();
        return repetition;
    }

    public static async Task<Activity> AddActivityAsync(DBContext context, Activity activity)
    {
        await context.Activity.AddAsync(activity);
        await context.SaveChangesAsync();
        return activity;
    }
}