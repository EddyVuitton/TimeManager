using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Components;

public partial class MenuTask
{
    [Inject] public IDialogService DialogService { get; private init; } = null!;

    [CascadingParameter] public MudMenu MudMenu { get; set; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public TaskList TaskListRef { get; init; } = null!;
    [Parameter] public RepetitionDto RepetitionDto { get; init; } = null!;

    private void CloseMenu()
    {
        MudMenu.CloseMenu();
    }

    private void OpenAddListDialog()
    {
        CloseMenu();

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters
        {
            { "TasksRef", TasksRef },
            { "RepetitionId", RepetitionDto.RepetitionId }
        };

        DialogService.Show<AddListDialog>("Tworzenie nowej listy", parameters, options);
    }

    public async Task MoveRepetitionToList(int taskListId)
    {
        await TaskListRef.MoveRepetitionToList(RepetitionDto.RepetitionId, taskListId);
        CloseMenu();
    }

    public async Task RemoveRepetition()
    {
        await TaskListRef.RemoveRepetition(RepetitionDto.RepetitionId);
        CloseMenu();
    }
}