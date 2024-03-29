﻿using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;

namespace TimeManager.WebUI.Popovers;

public partial class ActivityListPopover
{
    [Parameter] public Activity ActivityRef {get; init; } = null!;

    private List<ActivityDto> _activitiesDto = null!;
    private string _shortDayWeekName = null!;
    private int _dayNumber;

    protected override void OnInitialized()
    {
        InitComponent();
    }

    private void OpenActivityPopover(ActivityDto activity)
    {
        foreach (var a in _activitiesDto)
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

    private void ClosePopover() =>
        ActivityRef.ToggleActivityListPopover();

    public void InitComponent()
    {
        _activitiesDto = ActivityRef.ActivitiesDto;
        _shortDayWeekName = BasicHelper.GetShortDayWeekName((int)ActivityRef.Day.DayDto.Day.DayOfWeek).ToUpper();
        _dayNumber = ActivityRef.Day.DayDto.Day.Day;
        StateHasChanged();
    }
}