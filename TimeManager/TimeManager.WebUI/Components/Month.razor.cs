using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.WebAPI.APIs.Management;
using TimeManager.WebUI.Helpers;

namespace TimeManager.WebUI.Components;

public partial class Month
{
    [Inject] public IManagementService ManagementService { get; set; } = null!;

    [CascadingParameter(Name = "UserId")] public int UserId { get; set; }

    private MonthDto _monthDto = new();

    private readonly DateTime _now = DateTime.Now;
    private DateTime _lastDayOfMonth;
    private int _daysInMonth;
    private int _daysInLastWeek;
    private int _weeks;
    private string _monthName = string.Empty;

    private List<ActivityDto> _allActivitiesDto = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LoadActivitiesAsync();
            InitMonth(new DateTime(_now.Year, _now.Month, 1));
        }
        catch
        {
        }
    }

    #region PrivateMethods

    private async Task LoadActivitiesAsync()
    {
        var userActivities = await ManagementService.GetActivitiesAsync(UserId);

        if (!userActivities.IsSuccess)
        {
            throw new Exception(userActivities.Message ?? "Błąd we wczytaniu aktywności...");
        }

        _allActivitiesDto = userActivities.Data;
    }

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

    public async void AddActivity(ActivityDto activity)
    {
        try
        {
            var newActivityResult = await ManagementService.AddActivityAsync(activity);

            if (!newActivityResult.IsSuccess)
            {
                throw new Exception(newActivityResult.Message ?? "Błąd w dodaniu aktywności...");
            }

            _allActivitiesDto.Add(newActivityResult.Data);
        }
        catch
        {
        }
        finally
        {
            InitMonth(activity.Day);
        }
    }

    public void RemoveActivity(ActivityDto activity)
    {
        _allActivitiesDto.Remove(activity);
        InitMonth(activity.Day);
    }

    #endregion PublicMethods
}