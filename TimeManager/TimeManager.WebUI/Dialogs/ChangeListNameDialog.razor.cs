using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Dialogs;

public partial class ChangeListNameDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public ActivityListDto ListDto { get; init; } = null!;

    private readonly string _errorText = "Nazwa listy zadań nie może być pusta.";
    private bool isError = false;

    private async Task Submit()
    {
        if (string.IsNullOrEmpty(ListDto.Name))
        {
            isError = true;
            return;
        }

        await TasksRef.ChangeListName(ListDto);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}