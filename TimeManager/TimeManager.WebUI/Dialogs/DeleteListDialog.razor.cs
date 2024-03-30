﻿using MudBlazor;
using TimeManager.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using TimeManager.WebUI.Pages;

namespace TimeManager.WebUI.Dialogs;

public partial class DeleteListDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public Tasks TasksRef { get; init; } = null!;
    [Parameter] public ListDto ListDto { get; init; } = null!;

    private void Submit()
    {
        TasksRef.DeleteList(ListDto.ID);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}