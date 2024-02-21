using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;

namespace TimeManager.WebUI.Components;

public partial class Activity
{
    [CascadingParameter(Name = "MonthRef")] public Month MonthRef { get; set; } = null!;

    [Parameter] public Day Day { get; set; } = null!;
    [Parameter] public List<ActivityDto> ActivitiesDto { get; set; } = null!;

    private static void OpenPopover(ActivityDto activity) =>
        activity.IsOpen = !activity.IsOpen;

    public async Task RemoveActivity(ActivityDto activity)
    {
        await MonthRef.RemoveActivity(activity);
        Day.DayStateHasChanged();
    }
}