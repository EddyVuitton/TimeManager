using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.Domain.DTOs;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Dialogs;

public partial class ChangeListNameDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public ListDto ListDto { get; init; } = null!;

    private readonly string _errorText = "Nazwa listy zadań nie może być pusta.";
    private bool isError = false;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private void Submit()
    {
        if (string.IsNullOrEmpty(ListDto.Name))
        {
            isError = true;
            return;
        }

        TasksRef.ChangeListName(ListDto);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}