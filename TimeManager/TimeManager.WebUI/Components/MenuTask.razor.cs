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
    [Parameter] public ActivityDto TaskDto { get; init; } = null!;

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
            { "TaskId", TaskDto.ActivityId }
        };

        DialogService.Show<AddListDialog>("Tworzenie nowej listy", parameters, options);
    }

    public async Task MoveTaskToList(int taskListId)
    {
        await TaskListRef.MoveTaskToList(TaskDto.ActivityId, taskListId);
        CloseMenu();
    }

    public async Task RemoveTask()
    {
        await TaskListRef.RemoveTask(TaskDto.ActivityId);
        CloseMenu();
    }
}