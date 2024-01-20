using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Components.Dialogs;
using TimeManager.Data.DTOs;

namespace TimeManager.Components.Components;

public partial class Day
{
    [Inject] public IDialogService? DialogService { get; set; }

    [Parameter] public DateTime DayBody { get; set; }
    [Parameter] public Month? MonthRef { get; set; }

    private string _dayText = string.Empty;
    private List<ActivityDto> _activitiesDto = new();

    protected override void OnInitialized()
    {
        InitFields();
    }

    #region PrivateMethods
    private void InitFields()
    {
        _dayText = DayBody.Day < 10 ? $"0{DayBody.Day}" : DayBody.Day.ToString();
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
            { "DayBody", DayBody },
            { "DayRef", this }
        };

        DialogService?.Show<AddActivity>(string.Empty, parameters, options);
    }
    #endregion PrivateMethods

    #region PublicMethods
    public void AddActivity(ActivityDto activity)
    {
        _activitiesDto.Add(activity);
        StateHasChanged();
    }
    #endregion PublicMethods
}