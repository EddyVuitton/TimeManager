using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
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

    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = await LoginService.GetUserIdFromToken();
        lists = (await ManagementService.GetActivityListsAsync(_userId)).Data;
        await LoadActivitiesAsync();
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

    public void ChangeListName(ActivityListDto modifiedList)
    {
        var list = lists.First(x => x.ID == modifiedList.ID);
        list.Name = modifiedList.Name;

        StateHasChanged();
    }

    public void DeleteList(int id)
    {
        var list = lists.First(x => x.ID == id);
        lists.Remove(list);

        StateHasChanged();
    }

    #endregion PublicMethods
}