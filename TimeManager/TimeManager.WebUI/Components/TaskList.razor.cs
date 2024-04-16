using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

    private MudMenu _mudMenuRef = null!;
    private bool _isOpenPopoverMenu = false;
    private string? _popoverStyle;

    #region PrivateMethods

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

    private void TogglePopoverMenu(MouseEventArgs args)
    {
        if (_isOpenPopoverMenu)
        {
            ClosePopoverMenu();
        }
        else
        {
            OpenPopoverMenu(args);
        }
    }

    private void SetPopoverStyle(MouseEventArgs args)
    {
        var clientX = args?.ClientX.ToString("0.##");
        var clientY = args?.ClientY.ToString("0.##");
        _popoverStyle = $"padding: 10px 0px; position: fixed !important; left: {clientX}px; top: {clientY}px;";
    }

    private void OpenPopoverMenu(MouseEventArgs args)
    {
        if (_mudMenuRef.IsOpen)
        {
            _mudMenuRef.CloseMenu();
        }

        SetPopoverStyle(args);
        _isOpenPopoverMenu = true;

        StateHasChanged();
    }

    #endregion PrivateMethods

    #region PublicMethods

    public async Task MoveRepetitionToList(int id, int taskListId)
    {
        await TasksRef.MoveRepetitionToList(id, taskListId);
        StateHasChanged();
    }

    public async Task RemoveRepetition(int id)
    {
        await TasksRef.RemoveRepetition(id);
        StateHasChanged();
    }

    public void ClosePopoverMenu()
    {
        _isOpenPopoverMenu = false;
        _popoverStyle = null;
        StateHasChanged();
    }

    #endregion PublicMethods
}