using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;
using TimeManager.WebUI.Auth;
using TimeManager.WebUI.Dialogs;
using TimeManager.WebUI.Services.Management;
using TimeManager.WebUI.Services.Snackbar;

namespace TimeManager.WebUI.Pages;

public partial class Tasks
{
    [Inject] public IDialogService DialogService { get; private init; } = null!;
    [Inject] public ILoginService LoginService { get; private init; } = null!;
    [Inject] public IManagementService ManagementService { get; private init; } = null!;
    [Inject] public ISnackbarService SnackbarService { get; private init; } = null!;

    private List<ActivityListDto> lists = [];
    private List<ActivityDto> _allActivitiesDto = [];
    private List<HourType> _hourTypes = [];
    private List<RepetitionType> _repetitionTypes = [];

    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = await LoginService.GetUserIdFromToken();
        lists = (await ManagementService.GetActivityListsAsync(_userId)).Data;
        await LoadActivitiesAsync();
        await LoadHourTypesAsync();
        await LoadRepetitionTypesAsync();
        LoadTasksToLists();
    }

    #region PrivateMethods

    private void OpenAddListDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters
        {
            { "TasksRef", this }
        };

        DialogService.Show<AddListDialog>("Tworzenie nowej listy", parameters, options);
    }

    private async Task LoadActivitiesAsync()
    {
        var userActivities = await ManagementService.GetActivitiesAsync(_userId);

        if (!userActivities.IsSuccess)
        {
            throw new Exception(userActivities.Message ?? "Błąd we wczytaniu aktywności...");
        }

        _allActivitiesDto = userActivities.Data;
    }

    private void LoadTasksToLists()
    {
        foreach (var list in lists)
        {
            list.Tasks = _allActivitiesDto.Where(x => x.ActivityListId == list.ID).ToList();
        }
    }

    private async Task LoadHourTypesAsync()
    {
        try
        {
            var hourTypesResult = await ManagementService.GetHourTypesAsync();

            if (!hourTypesResult.IsSuccess)
            {
                throw new Exception(hourTypesResult.Message ?? "Błąd w pobraniu godzin do wyboru...");
            }

            _hourTypes = hourTypesResult.Data;
        }
        catch
        {
            //handle exceptions...
        }
    }

    private async Task LoadRepetitionTypesAsync()
    {
        try
        {
            var repetitionTypesResult = await ManagementService.GetRepetitionTypesAsync();

            if (!repetitionTypesResult.IsSuccess)
            {
                throw new Exception(repetitionTypesResult.Message ?? "Błąd w pobraniu typów powtórzeń...");
            }

            _repetitionTypes = repetitionTypesResult.Data;
        }
        catch
        {
            //handle exceptions...
        }
    }

    #endregion PrivateMethods

    #region PublicMethods

    public async Task AddList(string name)
    {
        var newActivityList = new ActivityListDto() { Name = name, IsChecked = true, UserId = _userId };

        try
        {
            var newActivityListResult = await ManagementService.AddActivityListAsync(newActivityList);

            if (!newActivityListResult.IsSuccess)
            {
                throw new Exception(newActivityListResult.Message ?? "Błąd w dodaniu listy...");
            }

            lists.Add(newActivityListResult.Data);

            SnackbarService.Show("Utworzono listę", Severity.Normal, true, false);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }

        StateHasChanged();
    }

    public async Task ChangeListName(ActivityListDto modifiedList)
    {
        try
        {
            var updatedActivityListResult = await ManagementService.UpdateActivityListAsync(modifiedList);

            if (!updatedActivityListResult.IsSuccess)
            {
                throw new Exception(updatedActivityListResult.Message ?? "Błąd w dodaniu listy...");
            }

            var list = lists.First(x => x.ID == modifiedList.ID);
            list.Name = modifiedList.Name;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
    }

    public async Task DeleteList(int id)
    {
        try
        {
            var removedActivityListResult = await ManagementService.RemoveActivityListAsync(id);

            if (!removedActivityListResult.IsSuccess)
            {
                throw new Exception(removedActivityListResult.Message ?? "Błąd w usunięciu listy...");
            }

            var list = lists.First(x => x.ID == id);
            lists.Remove(list);

            SnackbarService.Show("Lista zadań została usunięta", Severity.Normal, true, false);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
    }

    public async Task AddActivity(ActivityDto activity)
    {
        try
        {
            var newActivityResult = await ManagementService.AddActivityAsync(activity);

            if (!newActivityResult.IsSuccess)
            {
                throw new Exception(newActivityResult.Message ?? "Błąd w dodaniu zadania...");
            }

            _allActivitiesDto.Add(activity);
            LoadTasksToLists();
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }

        StateHasChanged();
    }

    public List<HourType> GetHourTypes() => _hourTypes;

    public List<RepetitionType> GetRepetitionTypes() => _repetitionTypes;

    public int GetUserId() => _userId;

    #endregion PublicMethods
}