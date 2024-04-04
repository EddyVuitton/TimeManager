using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Dialogs;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Components;

public partial class TaskList
{
    [Inject] public IDialogService DialogService { get; init; } = null!;

    [Parameter] public ActivityListDto ActivityList { get; init; } = null!;
    [Parameter] public Tasks TasksRef { get; init; } = null!;

    private void OpenAddActivityDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
        };

        var parameters = new DialogParameters
        {
            { "TasksRef", TasksRef},
            { "HourTypes", TasksRef.GetHourTypes()},
            { "RepetitionTypes", TasksRef.GetRepetitionTypes() },
            { "ActivityList", ActivityList }
        };

        DialogService.Show<AddActivityFromListDialog>(string.Empty, parameters, options);
    }
}