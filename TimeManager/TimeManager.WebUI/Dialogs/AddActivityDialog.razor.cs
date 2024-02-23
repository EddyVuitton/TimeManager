using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebAPI.APIs.Management.Interfaces;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;

namespace TimeManager.WebUI.Dialogs;

public partial class AddActivityDialog
{
    [Inject] public IManagementService ManagementService { get; set; } = null!;

    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public DayDto DayDto { get; set; } = null!;
    [Parameter] public Day DayRef { get; set; } = null!;

    private DateTime _now;
    private Dictionary<int, string> _hourTypeList = null!;
    private Dictionary<int, string> _repetitionTypeList = null!;
    private Dictionary<int, string> _activityLists = null!;

    private string? _title;
    private string _description = null!;
    private string _dayName = null!;
    
    private bool _showAddDeadlineButton;
    private bool _isRepetitionSelectDisabled;

    private int _repetitionTypeId;
    private int _hourTypeId;
    private int _activityListId;

    protected override async Task OnInitializedAsync()
    {
        InitFields();
        SetDayName();
        await AddRepetitionsToSelectAsync();
        await AddHoursToSelectAsync();
        AddActivityListToSelect();
        _repetitionTypeId = _repetitionTypeList.FirstOrDefault().Key;
        _activityListId = _activityLists.FirstOrDefault().Key;
    }

    #region PrivateMethods

    private void InitFields()
    {
        _description = string.Empty;
        _hourTypeList = [];
        _now = DateTime.Now;
        _dayName = string.Empty;
        _showAddDeadlineButton = false;
        _isRepetitionSelectDisabled = true;
    }

    private async Task AddHoursToSelectAsync()
    {
        _hourTypeList = [];
        try
        {
            var hourTypesResult = await ManagementService.GetHourTypesAsync();

            if (!hourTypesResult.IsSuccess)
            {
                throw new Exception(hourTypesResult.Message ?? "Błąd w pobraniu godzin do wyboru...");
            }

            foreach (var type in hourTypesResult.Data)
            {
                _hourTypeList.Add(type.Id, type.Name);
            }
        }
        catch
        {
        }
    }

    private async Task AddRepetitionsToSelectAsync()
    {
        _repetitionTypeList = [];
        try
        {
            var repetitionTypesResult = await ManagementService.GetRepetitionTypesAsync();

            if (!repetitionTypesResult.IsSuccess)
            {
                throw new Exception(repetitionTypesResult.Message ?? "Błąd w pobraniu typów powtórzeń...");
            }

            foreach (var type in repetitionTypesResult.Data)
            {
                _repetitionTypeList.Add(type.Id, type.Name);
            }
        }
        catch
        {
        }
    }

    private void AddActivityListToSelect()
    {
        _activityLists = [];
        var userActivityLists = DayRef.MonthRef.GetActivityLists();
        foreach (var a in userActivityLists)
        {
            _activityLists.Add(a.Id, a.Name);
        }
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
        if (_hourTypeId == 0)
            _hourTypeId = 1;

        var activity = new ActivityDto()
        {
            Day = DayDto.Day,
            Title = _title,
            Description = _description,
            HourTypeId = _hourTypeId,
            RepetitionTypeId = _repetitionTypeId,
            ActivityListId = _activityListId
        };

        DayRef.AddActivity(activity);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion PrivateMethods
}