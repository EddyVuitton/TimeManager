using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;

namespace TimeManager.WebUI.Dialogs;

public partial class AddActivityDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public DayDto DayDto { get; set; } = null!;
    [Parameter] public Day DayRef { get; set; } = null!;
    [Parameter] public Dictionary<int, string> HourTypeList { get; set; } = null!;
    [Parameter] public Dictionary<int, string> RepetitionTypeList { get; set; } = null!;
    [Parameter] public Dictionary<int, string> ActivityLists { get; set; } = null!;

    private string? _title;
    private string _description = null!;
    private string _dayName = null!;

    private bool _showAddDeadlineButton;
    private bool _isRepetitionSelectDisabled;

    private int _repetitionTypeId;
    private int? _hourTypeId;
    private int _activityListId;

    protected override void OnInitialized()
    {
        InitFields();
        SetDayName();
    }

    #region PrivateMethods

    private void InitFields()
    {
        _description = string.Empty;
        _dayName = string.Empty;
        _showAddDeadlineButton = false;
        _isRepetitionSelectDisabled = true;
        _repetitionTypeId = RepetitionTypeList.FirstOrDefault().Key;
        _activityListId = ActivityLists.FirstOrDefault().Key;
    }

    private void SetDayName()
    {
        var monthName = BasicHelper.GetMonthName(DayDto.Day.Month)[..3].ToLower() ?? string.Empty;
        _dayName = $"{DayDto.Day.Day} {monthName} {DayDto.Day.Year}";
    }

    private void ShowAddDeadlineButton()
    {
        _showAddDeadlineButton = true;
        _isRepetitionSelectDisabled = false;
    }

    private void OnRepetitionChange(ChangeEventArgs e) =>
        _repetitionTypeId = int.Parse(e.Value as string ?? string.Empty);

    private void OnHourChange(ChangeEventArgs e) =>
        _hourTypeId = int.Parse(e.Value as string ?? string.Empty);

    private void OnActivityListChange(ChangeEventArgs e) =>
        _activityListId = int.Parse(e.Value as string ?? string.Empty);

    private void Submit()
    {
        var activity = new ActivityDto()
        {
            Day = DayDto.Day,
            Title = _title,
            Description = _description,
            HourTypeId = _hourTypeId ?? 1,
            RepetitionTypeId = _repetitionTypeId,
            ActivityListId = _activityListId
        };

        DayRef.AddActivity(activity);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion PrivateMethods
}