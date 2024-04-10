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
    private List<HourType> _hourTypes = [];
    private List<RepetitionType> _repetitionTypes = [];
    private List<RepetitionDto> _repetitions = [];

    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = await LoginService.GetUserIdFromToken();
        await LoadActivityListsAsync();
        await LoadRepetitionsAsync();
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

    private void LoadDataToLists()
    {
        foreach (var list in _lists)
        {
            list.Repetitions = _repetitions.Where(x => x.ActivityListId == list.ID).ToList();
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

    private async Task LoadRepetitionsAsync()
    {
        var repetitionsResult = await ManagementService.GetRepetitionsAsync(_userId);

        if (!repetitionsResult.IsSuccess)
        {
            throw new Exception(repetitionsResult.Message ?? "Błąd we wczytaniu powtórzeń...");
        }

        _repetitions = repetitionsResult.Data;
        LoadDataToLists();
    }

    #endregion PrivateMethods

    #region PublicMethods

    public async Task AddList(string name, int repetitionId = 0)
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

            if (repetitionId != 0)
            {
                await MoveRepetitionToList(repetitionId, newActivityListResult.Data.ID);
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

            await LoadRepetitionsAsync();

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

    public async Task MoveRepetitionToList(int id, int taskListId)
    {
        var repetition = _repetitions.First(x => x.RepetitionId == id);

        if (repetition.ActivityListId == taskListId)
            return;

        try
        {
            var moveRepetitionToListResult = await ManagementService.MoveRepetitionToListAsync(id, taskListId);

            if (!moveRepetitionToListResult.IsSuccess)
            {
                throw new Exception(moveRepetitionToListResult.Message ?? "Błąd w przeniesieniu zadania do innej listy...");
            }

            repetition.ActivityListId = taskListId;
            LoadDataToLists();

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
    }

    public async Task RemoveRepetition(int id)
    {
        try
        {
            var removeRepetitionResult = await ManagementService.RemoveRepetitionAsync(id);

            if (!removeRepetitionResult.IsSuccess)
            {
                throw new Exception(removeRepetitionResult.Message ?? "Błąd w usunięciu aktywności..");
            }

            _repetitions.RemoveAll(x => x.RepetitionId == id);

            LoadDataToLists();

            SnackbarService.Show("Usunięto zadanie", Severity.Normal, true, false);
        }
        catch (Exception ex)
        {
            SnackbarService.Show(ex.Message, Severity.Warning, true, false);
        }
    }

    #endregion PublicMethods
}