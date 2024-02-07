using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;
using TimeManager.Domain.DTOs;

namespace TimeManager.WebUI.Dialogs;

public partial class AddActivityDialog
{
    [CascadingParameter] public MudDialogInstance? MudDialog { get; set; }

    [Parameter] public DayDto? DayDto { get; set; }
    [Parameter] public Day? DayRef { get; set; }

    private readonly string[] _taskLists = { "Moje zadania" };
    private readonly List<string> _hourList = new();
    private readonly List<string> _repetitionList = new();
    private readonly DateTime _now = DateTime.Now;

    private string _description = string.Empty;
    private string _title = string.Empty;
    private string _taskValue = "Moje zadania";
    private string _hourValue = string.Empty;
    private string _repetitionValue = "Nie powtarza się";
    private string _dayName = string.Empty;

    private bool _showAddDeadlineButton = false;
    private bool _isRepetitionSelectDisabled = true;

    protected override void OnInitialized()
    {
        AddHoursToSelect();
        AddRepetitionsToSelect();
        SetDayName();
    }

    #region PrivateMethods

    private void AddHoursToSelect()
    {
        var start = new DateTime(_now.Year, _now.Month, _now.Day, 0, 0, 0);

        for (int i = 0; i < 96; i++)
        {
            var hour = start.Hour < 10 ? $"0{start.Hour}" : start.Hour.ToString();
            var minute = start.Minute < 10 ? $"0{start.Minute}" : start.Minute.ToString();

            _hourList.Add($"{hour}:{minute}");
            start = start.AddMinutes(15);
        }
    }

    private void AddRepetitionsToSelect()
    {
        _repetitionList.Add("Nie powtarza się");
        _repetitionList.Add("Codziennie");
        _repetitionList.Add("Co tydzień w: wtorek");
        _repetitionList.Add("Co miesiąc w: 3. wtorek");
        _repetitionList.Add("Co roku w dniu: 16 stycznia");
        _repetitionList.Add("W dni powszednie (od poniedziałku do piątku)");
    }

    private void SetDayName()
    {
        var monthName = BasicHelper.GetMonthName(DayDto!.Day.Month)[..3].ToLower() ?? string.Empty;
        _dayName = $"{DayDto.Day.Day} {monthName} {DayDto.Day.Year}";
    }

    private void ShowAddDeadlineButton()
    {
        _showAddDeadlineButton = true;
        _isRepetitionSelectDisabled = false;
    }

    private void OnRepetitionChange(ChangeEventArgs e) =>
        _repetitionValue = e.Value as string ?? string.Empty;

    private void OnHourChange(ChangeEventArgs e) =>
        _hourValue = e.Value as string ?? string.Empty;

    private void OnTaskChange(ChangeEventArgs e) =>
        _taskValue = e.Value as string ?? string.Empty;

    private void Submit()
    {
        var activity = new ActivityDto()
        {
            Day = DayDto!.Day,
            Title = _title,
            Description = _description,
            Task = _taskValue,
            Hour = _hourValue,
            RepetitionType = _repetitionValue
        };

        DayRef?.AddActivity(activity);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog?.Cancel();

    #endregion PrivateMethods
}