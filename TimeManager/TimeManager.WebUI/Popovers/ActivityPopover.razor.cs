using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Popovers;

public partial class ActivityPopover
{
    [CascadingParameter(Name = "HomeRef")] protected Home HomeRef { get; set; } = null!;

    [Parameter] public Activity ActivityRef { get; set; } = null!;
    [Parameter] public ActivityDto ActivityDto { get; set; } = null!;

    private string _dayName = string.Empty;
    private bool isReadonly = true;
    private const string _TITLEEDITABLE = "font-size: 2rem; color: black;";
    private const string _TITLEUNEDITABLE = "font-size: 2rem; color: #969696;";
    private string _titleStyle = string.Empty;
    private string? _placeholder;
    private string _activityListName = null!;

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
        _activityListName = ActivityRef.MonthRef.GetActivityLists().First(x => x.Key == ActivityDto.ActivityListId).Value;
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
        await ActivityRef.RemoveActivity(ActivityDto);
        StateHasChanged();
    }

    #endregion PrivateMethods
}