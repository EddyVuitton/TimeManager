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

    private List<ActivityListDto> _lists = [];
    private List<ActivityDto> _allActivitiesDto = [];
    private List<HourType> _hourTypes = [];
    private List<RepetitionType> _repetitionTypes = [];

    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = await LoginService.GetUserIdFromToken();
        await LoadActivityListsAsync();
        await LoadActivitiesAsync();
        await LoadHourTypesAsync();
        await LoadRepetitionTypesAsync();
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
        LoadTasksToLists();
    }

    private async Task LoadActivityListsAsync()
    {
        try
        {
            var activityListsResult = await ManagementService.GetActivityListsAsync(_userId);

            if (!activityListsResult.IsSuccess)
            {
                throw new Exception(activityListsResult.Message ?? "Błąd w pobraniu list...");
            }

            _lists = activityListsResult.Data;
            StateHasChanged();
        }
        catch
        {
            //handle exceptions...
        }
    }

    private void LoadTasksToLists()
    {
        foreach (var list in _lists)
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

    public async Task AddList(string name, int taskId = 0)
    {
        var newActivityList = new ActivityListDto() { Name = name, IsChecked = true, UserId = _userId };

        try
        {
            var newActivityListResult = await ManagementService.AddActivityListAsync(newActivityList);

            if (!newActivityListResult.IsSuccess)
            {
                throw new Exception(newActivityListResult.Message ?? "Błąd w dodaniu listy...");
            }

            _lists.Add(newActivityListResult.Data);

            if (taskId != 0)
            {
                await MoveTaskToList(taskId, newActivityListResult.Data.ID);
            }

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

            var list = _lists.First(x => x.ID == modifiedList.ID);
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

            var list = _lists.First(x => x.ID == id);
            _lists.Remove(list);

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
            //await LoadActivityListsAsync();
            //LoadTasksToLists();

            await LoadActivitiesAsync();

            // await OnInitializedAsync();

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
    }

    public List<HourType> GetHourTypes() => _hourTypes;

    public List<RepetitionType> GetRepetitionTypes() => _repetitionTypes;

    public int GetUserId() => _userId;

    public List<ActivityListDto> GetActivityLists() => _lists;

    public async Task MoveTaskToList(int taskId, int taskListId)
    {
        var task = _allActivitiesDto.First(x => x.ActivityId == taskId);

        if (task.ActivityListId == taskListId)
            return;

        try
        {
            var moveTaskToListResult = await ManagementService.MoveTaskToList(taskId, taskListId);

            if (!moveTaskToListResult.IsSuccess)
            {
                throw new Exception(moveTaskToListResult.Message ?? "Błąd w przeniesieniu zadania do innej listy...");
            }

            task.ActivityListId = taskListId;
            LoadTasksToLists();

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
    }

    public async Task RemoveTask(int taskId)
    {
        var task = _allActivitiesDto.FirstOrDefault(x => x.ActivityId == taskId);
        if (task is not null)
        {
            try
            {
                var removeActivityResult = await ManagementService.RemoveActivityAsync(taskId);

                if (!removeActivityResult.IsSuccess)
                {
                    throw new Exception(removeActivityResult.Message ?? "Błąd w usunięciu aktywności..");
                }

                _allActivitiesDto.Remove(task);
                LoadTasksToLists();

                SnackbarService.Show("Usunięto zadanie", Severity.Normal, true, false);
            }
            catch (Exception ex)
            {
                SnackbarService.Show(ex.Message, Severity.Warning, true, false);
            }
        }
    }

    #endregion PublicMethods
}