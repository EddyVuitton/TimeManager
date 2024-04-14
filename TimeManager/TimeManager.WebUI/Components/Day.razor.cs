using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;
using TimeManager.WebUI.Dialogs.Auth;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Components;

public partial class Day
{
    [Inject] public IDialogService DialogService { get; init; } = null!;
    [Inject] public ISnackbarService SnackbarService { get; init; } = null!;

    [CascadingParameter(Name = "MonthRef")] public Month MonthRef { get; private init; } = null!;

    [Parameter] public DayDto DayDto { get; init; } = null!;
    [Parameter] public bool IsFirst7 { get; init; }
    [Parameter] public bool IsBeforeOrAfter { get; init; }

    #region PrivateMethods

    private void OpenDialog()
    {
        if (MonthRef.UserId > 0)
        {
            OpenAddActivityDialog();
        }
        else
        {
            OpenLoginDialog();
        }
    }

    private void OpenAddActivityDialog()
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

    private void OpenLoginDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<LoginDialog>(string.Empty, options);
        SnackbarService.Show("Przed dodaniem zadania zaloguj się lub stwórz konto", Severity.Info, true, false);
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void AddActivity(ActivityDto activity)
    {
        activity.UserId = MonthRef.UserId;
        MonthRef.AddActivity(activity);
        DayDto.Activities.Add(activity);

        StateHasChanged();
    }

    public void DayStateHasChanged()
        => StateHasChanged();

    #endregion PublicMethods
}