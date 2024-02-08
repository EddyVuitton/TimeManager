using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Components;
using TimeManager.WebUI.Helpers;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Popovers;

public partial class OpenActivityPopover
{
    [CascadingParameter(Name = "HomeRef")] protected Home? HomeRef { get; set; }

    [Parameter] public Activity? Activity { get; set; }
    [Parameter] public ActivityDto? ActivityDto { get; set; }

    private string _dayName = string.Empty;
    private bool isReadonly = true;
    private const string _TITLEEDITABLE = "font-size: 2rem; color: black;";
    private const string _TITLEUNEDITABLE = "font-size: 2rem; color: #969696;";
    private string _titleStyle = string.Empty;

    protected override void OnInitialized()
    {
        var DayBody = DateTime.Now;

        var dayWeekName = BasicHelper.GetDayWeekName((int)DayBody.DayOfWeek);
        var polishMonthInflection = BasicHelper.GetPolishMonthInflection(DayBody.Month).ToLower();
        _dayName = $"{dayWeekName}, {DayBody.Day} {polishMonthInflection}";
        _titleStyle = _TITLEUNEDITABLE;
    }

    #region PrivateMethods

    private void ToggleReadonly()
    {
        isReadonly = !isReadonly;
        _titleStyle = isReadonly ? _TITLEUNEDITABLE : _TITLEEDITABLE;
        StateHasChanged();
    }

    private void ToggleOpen()
    {
        ActivityDto!.ToggleOpen();
        isReadonly = true;
        _titleStyle = _TITLEUNEDITABLE;
        Activity!.Day!.DayStateHasChanged();
    }

    private void DeleteActivity()
    {
        Activity!.RemoveActivity(ActivityDto!);
        StateHasChanged();
    }

    #endregion PrivateMethods
}