using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Auth;
using TimeManager.WebUI.Dialogs;
using TimeManager.WebUI.Services.Management;

namespace TimeManager.WebUI.Pages;

public partial class Tasks
{
    [Inject] public IDialogService DialogService { get; private init; } = null!;
    [Inject] public ILoginService LoginService { get; private init; } = null!;
    [Inject] public IManagementService ManagementService { get; private init; } = null!;

    private List<ActivityListDto> lists = [];

    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = await LoginService.GetUserIdFromToken();
        lists = (await ManagementService.GetActivityListsDtoAsync(_userId)).Data;
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

    #endregion PrivateMethods

    #region PublicMethods

    public void AddList(string name)
    {
        var newId = lists.Count == 0 ? 0 : lists.Max(x => x.ID) + 1;
        lists.Add(new ActivityListDto() { ID = newId, Name = name, IsChecked = true });

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