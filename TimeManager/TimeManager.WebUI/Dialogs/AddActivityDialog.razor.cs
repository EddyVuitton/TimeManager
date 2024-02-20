using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
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

    private Dictionary<int, string> _hourTypeList = null!;
    private Dictionary<int, string> _repetitionTypeList = null!;
    private DateTime _now;

    private string[] _taskLists = null!;
    private string _description = null!;
    private string _taskValue = null!;
    private string _dayName = null!;
    private string? _title;
    private string? _repetitionTypeValue;

    private bool _showAddDeadlineButton = false;
    private bool _isRepetitionSelectDisabled = true;

    private int _repetitionTypeId;
    private int _hourTypeId;

    protected override async Task OnInitializedAsync()
    {
        InitFields();
        SetDayName();
        await AddRepetitionsToSelectAsync();
        await AddHoursToSelectAsync();
        SetDefualtRepetitionType();
    }

    #region PrivateMethods

    private void InitFields()
    {
        _taskLists = ["Moje zadania"];
        _description = string.Empty;
        _taskValue = "Moje zadania";
        _hourTypeList = [];
        _now = DateTime.Now;
        _dayName = string.Empty;
    }

    private void SetDefualtRepetitionType()
    {
        _repetitionTypeId = _repetitionTypeList.FirstOrDefault().Key;
        _repetitionTypeValue = _repetitionTypeList.FirstOrDefault(x => x.Key == _repetitionTypeId).Value;
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

    private void OnTaskChange(ChangeEventArgs e) =>
        _taskValue = e.Value as string ?? string.Empty;

    private void Submit()
    {
        if (_hourTypeId == 0)
            _hourTypeId = 1;

        var activity = new ActivityDto()
        {
            Day = DayDto.Day,
            Title = _title,
            Description = _description,
            Task = _taskValue,
            HourTypeId = _hourTypeId,
            RepetitionTypeId = _repetitionTypeId
        };

        DayRef.AddActivity(activity);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion PrivateMethods
}