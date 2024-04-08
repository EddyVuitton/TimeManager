using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Dialogs;

public partial class AddListDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public int TaskId { get; init; }

    private string _listName = string.Empty;
    private readonly string _errorText = "Nazwa listy zadań nie może być pusta.";
    private bool isError = false;

    private async Task Submit()
    {
        if (string.IsNullOrEmpty(_listName))
        {
            isError = true;
            return;
        }

        await TasksRef.AddList(_listName, TaskId);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}