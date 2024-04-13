using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Helpers;
using TimeManager.WebUI.Pages;
using TimeManager.WebUI.Services.Management;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Components;

public partial class Month
{
    [Inject] public IManagementService ManagementService { get; init; } = null!;
    [Inject] public ISnackbarService SnackbarService { get; init; } = null!;

    [Parameter] public Calendar CalendarRef { get; init; } = null!;

    public int UserId
    {
        get => _userId;
    }

    private MonthDto _monthDto = new();

    private readonly DateTime _now = DateTime.Now;
    private string _monthName = string.Empty;
    private int _userId;

    private List<ActivityDto> _allActivitiesDto = [];
    private List<DayDto> _days = [];
    private Dictionary<int, string> _activityLists = null!;
    private Dictionary<int, string> _hourTypeList = null!;
    private Dictionary<int, string> _repetitionTypeList = null!;

    protected override async Task OnInitializedAsync()
    {
        _userId = CalendarRef.GetUserId();

        try
        {
            await LoadHourTypesAsync();
            await LoadRepetitionTypesAsync();
            await LoadActivitiesAsync();
            await LoadActivityListsAsync();

            InitMonth(_now);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
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
        _activityLists = [];
        try
        {
            var activityListsResult = await ManagementService.GetActivityListsAsync(UserId);

            if (!activityListsResult.IsSuccess)
            {
                throw new Exception(activityListsResult.Message ?? "Błąd we wczytaniu list aktywności...");
            }

            foreach (var type in activityListsResult.Data)
            {
                _activityLists.Add(type.ID, type.Name);
            }
        }
        catch
        {
        }
    }

    private async Task LoadHourTypesAsync()
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

    private async Task LoadRepetitionTypesAsync()
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

    private void InitFields()
    {
        _monthName = BasicHelper.GetMonthName(_monthDto.Month);
        ClearAndFeelDays();

        StateHasChanged();
    }

    private void ClearAndFeelDays()
    {
        _days = [];

        var dayCountInMonth = _monthDto.Days.Count;
        var day = 1;
        var month = _monthDto.Month;
        var year = _monthDto.Year;

        while (day <= dayCountInMonth)
        {
            var newDay = new DateTime(year, month, day);
            _days.Add(new DayDto
            {
                Day = newDay,
                Activities = _allActivitiesDto.Where(x => x.Day == newDay).ToList()
            });

            day++;
        }

        var firstDay = _days.First().Day;
        var daysBeforeCurrentMonth = (int)firstDay.DayOfWeek;

        var c = _days.Count + daysBeforeCurrentMonth;

        var daysAfterCurrentMonth = c > 35 ? 42 - c : 35 - c;

        if (daysBeforeCurrentMonth > 0)
        {
            var tempMonth = firstDay.AddDays(-1).Month;
            var tempYear = firstDay.AddDays(-1).Year;
            var lastDayOfMonth = new DateTime(tempYear, tempMonth, 1).AddMonths(1).AddDays(-1).Day;
            var tempDay = lastDayOfMonth;

            for (int i = 0; i < daysBeforeCurrentMonth; i++)
            {
                var newDay = new DateTime(tempYear, tempMonth, tempDay);
                _days.Add(new DayDto
                {
                    Day = newDay,
                    Activities = _allActivitiesDto.Where(x => x.Day == newDay).ToList()
                });

                tempDay--;
            }
        }

        if (daysAfterCurrentMonth > 0)
        {
            var tempMonth = firstDay.AddMonths(1).Month;
            var tempYear = firstDay.AddMonths(1).Year;
            var tempDay = 1;

            for (int i = 0; i < daysAfterCurrentMonth; i++)
            {
                var newDay = new DateTime(tempYear, tempMonth, tempDay);
                _days.Add(new DayDto
                {
                    Day = newDay,
                    Activities = _allActivitiesDto.Where(x => x.Day == newDay).ToList()
                });

                tempDay++;
            }
        }

        _days = [.. _days.OrderBy(x => x.Day)];
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

            _allActivitiesDto.AddRange(newActivityResult.Data);
            SnackbarService.Show("Zadanie zostało zapisane", Severity.Normal, true, false);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
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
            SnackbarService.Show("Zadanie zostało usunięte", Severity.Normal, true, false);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
        finally
        {
            InitMonth(activity.Day);
        }
    }

    public async Task UpdateActivity(ActivityDto activity)
    {
        try
        {
            var updateActivityResult = await ManagementService.UpdateActivityAsync(activity);

            if (!updateActivityResult.IsSuccess)
            {
                throw new Exception(updateActivityResult.Message ?? "Błąd w zaktualizowaniu aktywności...");
            }

            var updatedActivity = _allActivitiesDto.FirstOrDefault(x => x.ActivityId == activity.ActivityId);
            updatedActivity = activity;
            SnackbarService.Show("Zadanie zostało zaktualizowane", Severity.Normal, true, false);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
        finally
        {
            InitMonth(activity.Day);
        }
    }

    public Dictionary<int, string> GetActivityLists() => _activityLists;

    public Dictionary<int, string> GetHourTypes() => _hourTypeList;

    public Dictionary<int, string> GetRepetitionTypes() => _repetitionTypeList;

    public List<ActivityDto> GetActivities() => _allActivitiesDto;

    #endregion PublicMethods
}