using Microsoft.AspNetCore.Components;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebAPI.APIs.Management.Interfaces;
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
    private List<ActivityList> _activityLists = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LoadActivitiesAsync();
            await LoadActivityListsAsync();
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

    private async Task LoadActivityListsAsync()
    {
        var activityLists = await ManagementService.GetActivityListsAsync(UserId);

        if (!activityLists.IsSuccess)
        {
            throw new Exception(activityLists.Message ?? "Błąd we wczytaniu list aktywności...");
        }

        _activityLists = activityLists.Data;
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

    public async Task RemoveActivity(ActivityDto activity)
    {
        try
        {
            var removeActivityResult = await ManagementService.RemoveActivityAsync(activity.ActivityId);

            if (!removeActivityResult.IsSuccess)
            {
                throw new Exception(removeActivityResult.Message ?? "Błąd w usunięciu aktywności...");
            }

            _allActivitiesDto.Remove(activity);
        }
        catch
        {
        }
        finally
        {
            InitMonth(activity.Day);
        }
    }

    public List<ActivityList> GetActivityLists() => _activityLists;

    #endregion PublicMethods
}