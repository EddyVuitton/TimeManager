using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;

namespace TimeManager.WebUI.Popovers;

public partial class ActivityPopover
{
    [Parameter] public Activity ActivityRef { get; init; } = null!;
    [Parameter] public ActivityDto ActivityDto { get; init; } = null!;

    private List<ActivityListDto> _activityLists = [];

    private string _dayName = string.Empty;
    private bool isReadonly = true;
    private const string _TITLEEDITABLE = "font-size: 2rem; color: black;";
    private const string _TITLEUNEDITABLE = "font-size: 2rem; color: #969696;";
    private string _titleStyle = string.Empty;
    private string? _placeholder;
    private string _activityListName = null!;
    private int _activityListId;

    protected override void OnInitialized()
    {
        InitFields();
    }

    #region PrivateMethods

    private void InitFields()
    {
        var dayBody = ActivityDto.Day;
        var dayWeekName = BasicHelper.GetDayWeekName((int)dayBody.DayOfWeek);
        var polishMonthInflection = BasicHelper.GetPolishMonthInflection(dayBody.Month).ToLower();
        _dayName = $"{dayWeekName}, {dayBody.Day} {polishMonthInflection}";
        _titleStyle = _TITLEUNEDITABLE;
        _placeholder = ActivityDto.Title ?? "(Bez tytułu)";
        _activityLists = ActivityRef.MonthRef.GetActivityLists();
        _activityListId = ActivityDto.ActivityListId;
        _activityListName = _activityLists.First(x => x.ID == _activityListId).Name;
    }

    private void ToggleReadonly()
    {
        isReadonly = !isReadonly;
        _titleStyle = isReadonly ? _TITLEUNEDITABLE : _TITLEEDITABLE;
        StateHasChanged();
    }

    private void ToggleOpen()
    {
        ActivityDto.IsOpen = !ActivityDto.IsOpen;
        isReadonly = true;
        _titleStyle = _TITLEUNEDITABLE;
        ActivityRef.Day.DayStateHasChanged();
    }

    private async Task DeleteActivity()
    {
        ActivityDto.IsOpen = false;
        await ActivityRef.RemoveActivity(ActivityDto);
        StateHasChanged();
    }

    private async Task OnTitleChange(string newTitle)
    {
        ActivityDto.Title = newTitle;
        await ActivityRef.UpdateActivity(ActivityDto);
        StateHasChanged();
    }

    private async Task OnDescriptionChange(string newDescription)
    {
        ActivityDto.Description = newDescription;
        await ActivityRef.UpdateActivity(ActivityDto);
        StateHasChanged();
    }

    private async Task OnActivityListChange(ChangeEventArgs e)
    {
        _activityListId = int.Parse(e.Value as string ?? string.Empty);
        ActivityDto.ActivityListId = _activityListId;
        await ActivityRef.UpdateActivity(ActivityDto);
        InitFields();
        StateHasChanged();
    }

    #endregion PrivateMethods
}