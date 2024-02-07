using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;

namespace TimeManager.WebUI.Components;

public partial class Activity
{
    [CascadingParameter(Name = "MonthRef")] public Month? MonthRef { get; set; }

    [Parameter] public Day? Day { get; set; }
    [Parameter] public List<ActivityDto>? ActivitiesDto { get; set; }

    private static void OpenPopover(ActivityDto activity) =>
        activity.IsOpen = !activity.IsOpen;

    public void RemoveActivity(ActivityDto activity)
    {
        MonthRef!.RemoveActivity(activity);
        Day!.DayStateHasChanged();
    }
}