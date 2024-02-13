using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Helpers;

namespace TimeManager.WebUI.Components;

public partial class Month
{
    private MonthDto _monthDto = new();

    private readonly DateTime _now = DateTime.Now;
    private DateTime _lastDayOfMonth;
    private int _daysInMonth;
    private int _daysInLastWeek;
    private int _weeks;
    private string _monthName = string.Empty;

    private readonly List<ActivityDto> _allActivitiesDto = [];

    protected override void OnInitialized()
    {
        InitMonth(new DateTime(_now.Year, _now.Month, 1));
    }

    #region PrivateMethods

    private void InitFields()
    {
        _lastDayOfMonth = new DateTime(_monthDto.Year, _monthDto.Month, 1).AddMonths(1).AddDays(-1);
        _daysInMonth = _lastDayOfMonth.Day;
        _daysInLastWeek = _daysInMonth % 7;
        _weeks = _daysInMonth / 7;
        _weeks += _daysInLastWeek > 0 ? 1 : 0;
        _monthName = BasicHelper.GetMonthName(_monthDto.Month);

        StateHasChanged();
    }

    private void IncreaseMonth()
    {
        var newDate = new DateTime(_monthDto.Year, _monthDto.Month, 1).AddMonths(1);
        InitMonth(newDate);
    }

    private void DecreaseMonth()
    {
        var newDate = new DateTime(_monthDto.Year, _monthDto.Month, 1).AddMonths(-1);
        InitMonth(newDate);
    }

    private void InitMonth(DateTime date)
    {
        var activities = BasicHelper.GetMonthActivities(_allActivitiesDto, date);
        _monthDto = BasicHelper.CreateMonthDto(date.Year, date.Month, activities);

        InitFields();
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void AddActivity(ActivityDto activity)
    {
        _allActivitiesDto.Add(activity);
        InitMonth(activity.Day);
    }

    public void RemoveActivity(ActivityDto activity)
    {
        _allActivitiesDto.Remove(activity);
        InitMonth(activity.Day);
    }

    #endregion PublicMethods
}