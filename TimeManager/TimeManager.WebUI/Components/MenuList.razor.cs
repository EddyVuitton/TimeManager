using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Components;

public partial class MenuList
{
    [Inject] public IDialogService DialogService { get; private init; } = null!;

    [CascadingParameter] public MudMenu MudMenu { get; set; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public ActivityListDto ListDto { get; init; } = null!;

    private void CloseMenu()
    {
        MudMenu.CloseMenu();
    }

    private void OpenChangeListNameDialog()
    {
        CloseMenu();

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters
        {
            { "TasksRef", TasksRef },
            { "ListDto", ListDto }
        };

        DialogService.Show<ChangeListNameDialog>("Zmiana nazwy listy", parameters, options);
    }

    private void OpenDeleteListDialog()
    {
        CloseMenu();

        if (ListDto.Tasks.Count == 0)
        {
            TasksRef.DeleteList(ListDto.ID);
        }
        else
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true
            };

            var parameters = new DialogParameters
        {
            { "TasksRef", TasksRef },
            { "ListDto", ListDto }
        };

            DialogService.Show<DeleteListDialog>("Usunąć tę listę?", parameters, options);
        }
    }
}