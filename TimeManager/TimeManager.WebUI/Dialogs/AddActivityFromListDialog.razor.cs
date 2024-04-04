using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Dialogs;

public partial class AddActivityFromListDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public List<HourType> HourTypes { get; init; } = null!;
    [Parameter] public List<RepetitionType> RepetitionTypes { get; init; } = null!;
    [Parameter] public ActivityListDto ActivityList { get; init; } = null!;

    private DateTime? _dayOfActivity;

    private string? _title;
    private string _description = null!;

    private bool _showAddDeadlineButton;
    private bool _isRepetitionSelectDisabled;

    private int _repetitionTypeId;
    private int? _hourTypeId;
    private int _activityListId;

    protected override void OnInitialized()
    {
        InitFields();
    }

    protected override void OnAfterRender(bool b)
    {
        if (_dayOfActivity is null)
        {
            _dayOfActivity = DateTime.Now;
            StateHasChanged();
        }
    }

    #region PrivateMethods

    private void InitFields()
    {
        _description = string.Empty;
        _showAddDeadlineButton = false;
        _isRepetitionSelectDisabled = true;
        _repetitionTypeId = RepetitionTypes.First().Id;
        _activityListId = ActivityList.ID;
        _dayOfActivity = DateTime.Now;
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

    private async Task Submit()
    {
        var activity = new ActivityDto()
        {
            Day = _dayOfActivity ?? DateTime.Now,
            Title = _title,
            Description = _description,
            HourTypeId = _hourTypeId ?? 1,
            RepetitionTypeId = _repetitionTypeId,
            ActivityListId = _activityListId,
            UserId = TasksRef.GetUserId()
        };

        await TasksRef.AddActivity(activity);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion PrivateMethods
}