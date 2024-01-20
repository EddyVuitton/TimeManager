using Microsoft.AspNetCore.Components;
using TimeManager.Components.Helpers;

namespace TimeManager.Components.Components;

public partial class Month
{
    [Parameter] public DateTime MonthBody { get; set; }

    private readonly List<DateTime> _days = new();

    private DateTime lastDayOfMonth;
    private int daysInMonth;
    private int daysInLastWeek;
    private int weeks;
    private string monthName = string.Empty;

    protected override void OnInitialized()
    {
        InitFields();
        StateHasChanged();
    }

    #region PrivateMethods
    private void InitFields()
    {
        lastDayOfMonth = new DateTime(MonthBody.Year, MonthBody.Month, 1).AddMonths(1).AddDays(-1);
        daysInMonth = lastDayOfMonth.Day;
        daysInLastWeek = daysInMonth % 7;
        weeks = daysInMonth / 7;
        weeks += daysInLastWeek > 0 ? 1 : 0;
        monthName = BasicHelper.GetMonthName(MonthBody.Month);

        GenerateDaysInMonth();
        StateHasChanged();
    }

    private void GenerateDaysInMonth()
    {
        if (_days is not null || _days?.Count > 0)
            _days.Clear();

        for (int i = 1; i <= daysInMonth; i++)
        {
            var day = new DateTime(MonthBody.Year, MonthBody.Month, i);
            _days?.Add(day);
        }
    }

    public void IncreaseMonth()
    {
        MonthBody = MonthBody.AddMonths(1);
        InitFields();
    }

    public void DecreaseMonth()
    {
        MonthBody = MonthBody.AddMonths(-1);
        InitFields();
    }
    #endregion PrivateMethods
}