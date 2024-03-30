using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;

namespace TimeManager.WebUI.Pages;

public partial class Tasks
{
    [Inject] public IDialogService DialogService { get; private init; } = null!;

    private readonly List<ListDto> lists = [];

    protected override void OnInitialized()
    {
        lists.Add(new ListDto() { ID = 0, Name = "Moje zadania", IsChecked = true });
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
        var newId = lists.Max(x => x.ID) + 1;
        lists.Add(new ListDto() { ID = newId, Name = name, IsChecked = true });

        StateHasChanged();
    }

    public void ChangeListName(ListDto modifiedList)
    {
        var list = lists.First(x => x.ID == modifiedList.ID);
        list.Name = modifiedList.Name;

        StateHasChanged();
    }

    #endregion PublicMethods
}