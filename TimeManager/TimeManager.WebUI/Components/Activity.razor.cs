using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Popovers;

namespace TimeManager.WebUI.Components;

public partial class Activity
{
    [CascadingParameter(Name = "MonthRef")] public Month MonthRef { get; private init; } = null!;

    [Parameter] public Day Day { get; init; } = null!;
    [Parameter] public List<ActivityDto> ActivitiesDto { get; init; } = null!;

    public ActivityListPopover? ActivityListPopoverRef { get; set; }

    public bool OpenActivityListPopover { get; set; } = false;

    private void OpenActivityPopover(ActivityDto activity)
    {
        OpenActivityListPopover = false;

        foreach (var a in ActivitiesDto)
        {
            if (a.Equals(activity))
            {
                a.IsOpen = !a.IsOpen;
            }
            else
            {
                a.IsOpen = false;
            }
        }
    }

    public void ToggleActivityListPopover()
    {
        OpenActivityListPopover = !OpenActivityListPopover;
        foreach (var a in ActivitiesDto)
        {
            a.IsOpen = false;
        }
    }

    public async Task RemoveActivity(ActivityDto activity)
    {
        await MonthRef.RemoveActivity(activity);
        StateHasChanged();
        Day.DayStateHasChanged();
    }

    public async Task UpdateActivity(ActivityDto activity)
    {
        await MonthRef.UpdateActivity(activity);
        Day.DayStateHasChanged();
    }

    private bool isVisible;
    private string _overlayStyle = "position: fixed;";

    public void OpenOverlay(MouseEventArgs args)
    {
        if (args.Button == 2)
        {
            _overlayStyle = "position: fixed;";
            _overlayStyle += $"left: {args.ClientX}px;";
            _overlayStyle += $"top: {args.ClientY}px;";
            isVisible = true;
        }

        StateHasChanged();
    }
}