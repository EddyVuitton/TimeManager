using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;

namespace TimeManager.WebUI.Components;

public partial class Day
{
    [Inject] public IDialogService? DialogService { get; set; }

    [CascadingParameter(Name = "MonthRef")] public Month? MonthRef { get; set; }

    [Parameter] public DayDto? DayDto { get; set; }

    private string _dayText = string.Empty;

    protected override void OnInitialized()
    {
        InitFields();
    }

    #region PrivateMethods

    private void InitFields()
    {
        _dayText = DayDto?.Day.Day < 10 ? $"0{DayDto?.Day.Day}" : DayDto?.Day.Day.ToString() ?? string.Empty;
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
            { "DayRef", this }
        };

        DialogService?.Show<AddActivityDialog>(string.Empty, parameters, options);
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void AddActivity(ActivityDto activity)
    {
        MonthRef!.AddActivity(activity);
        StateHasChanged();
    }

    public void RemoveActivity(ActivityDto activity)
    {
        MonthRef!.RemoveActivity(activity);
        StateHasChanged();
    }

    public void DayStateHasChanged() => StateHasChanged();

    #endregion PublicMethods
}