﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;

namespace TimeManager.WebUI.Components;

public partial class Day
{
    [Inject] public IDialogService DialogService { get; set; } = null!;

    [CascadingParameter(Name = "MonthRef")] public Month MonthRef { get; set; } = null!;

    [Parameter] public DayDto DayDto { get; set; } = null!;

    private string _dayText = string.Empty;

    protected override void OnInitialized()
    {
        InitFields();
    }

    #region PrivateMethods

    private void InitFields()
    {
        _dayText = DayDto.Day.Day.ToString();
    }

    private void OpenDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
        };

        var parameters = new DialogParameters
        {
            { "DayDto", DayDto },
            { "DayRef", this },
            { "HourTypeList", MonthRef.GetHourTypes() },
            { "RepetitionTypeList", MonthRef.GetRepetitionTypes() },
            { "ActivityLists", MonthRef.GetActivityLists() }
        };

        DialogService.Show<AddActivityDialog>(string.Empty, parameters, options);
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void AddActivity(ActivityDto activity)
    {
        activity.UserId = MonthRef.UserId;
        MonthRef.AddActivity(activity);
        StateHasChanged();
    }

    public void DayStateHasChanged() => StateHasChanged();

    #endregion PublicMethods
}