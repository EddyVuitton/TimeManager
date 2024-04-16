using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

    private bool _isOpenPopoverMenu = false;
    private string? _popoverStyle;

    #region PrivateMethods

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

    private void TogglePopoverActivityMenu(MouseEventArgs args)
    {
        if (_isOpenPopoverMenu)
        {
            ClosePopoverActivityMenu();
        }
        else
        {
            OpenPopoverActivityMenu(args);
        }
    }

    private void SetPopoverStyle(MouseEventArgs args)
    {
        var clientX = args?.ClientX.ToString("0.##");
        var clientY = args?.ClientY.ToString("0.##");
        _popoverStyle = $"width: 135px; padding: 10px 0px; position: fixed !important; left: {clientX}px; top: {clientY}px;";
    }

    private void OpenPopoverActivityMenu(MouseEventArgs args)
    {
        //if (_mudMenuRef.IsOpen)
        //{
        //    _mudMenuRef.CloseMenu();
        //}

        SetPopoverStyle(args);
        _isOpenPopoverMenu = true;

        StateHasChanged();
    }

    private void ClosePopoverActivityMenu()
    {
        _isOpenPopoverMenu = false;
        _popoverStyle = null;
        StateHasChanged();
    }

    #endregion PrivateMethods

    #region PublicMethods

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
        ClosePopoverActivityMenu();
    }

    public async Task UpdateActivity(ActivityDto activity)
    {
        await MonthRef.UpdateActivity(activity);
        Day.DayStateHasChanged();
    }

    #endregion PublicMethods
}