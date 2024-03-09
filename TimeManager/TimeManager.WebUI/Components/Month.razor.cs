using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Helpers;
using TimeManager.WebUI.Services.Management;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Components;

public partial class Month
{
    [Inject] public IManagementService ManagementService { get; set; } = null!;
    [Inject] public ISnackbarService SnackbarService { get; set; } = null!;

    [CascadingParameter(Name = "UserId")] public int UserId { get; set; }

    private MonthDto _monthDto = new();

    private readonly DateTime _now = DateTime.Now;
    private DateTime _lastDayOfMonth;
    private int _daysInMonth;
    private int _daysInLastWeek;
    private int _weeks;
    private string _monthName = string.Empty;

    private List<ActivityDto> _allActivitiesDto = [];
    private Dictionary<int, string> _activityLists = null!;
    private Dictionary<int, string> _hourTypeList = null!;
    private Dictionary<int, string> _repetitionTypeList = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LoadActivitiesAsync();
            await LoadActivityListsAsync();
            await LoadHourTypesAsync();
            await LoadRepetitionTypesAsync();
            InitMonth(new DateTime(_now.Year, _now.Month, 1));
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
                _activityLists.Add(type.Id, type.Name);
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
            SnackbarService.Show("Wydarzenie zostało zapisane", Severity.Normal, true, false);
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
            SnackbarService.Show("Wydarzenie zostało usunięte", Severity.Normal, true, false);
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
            SnackbarService.Show("Wydarzenie zostało zaktualizowane", Severity.Normal, true, false);
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