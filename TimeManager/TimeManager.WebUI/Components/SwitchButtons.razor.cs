using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TimeManager.WebUI.Components;

public partial class SwitchButtons
{
    [Inject] public NavigationManager NavigationManager { get; init; } = null!;

    private Variant _calendarVariant = Variant.Filled;
    private Variant _listVariant = Variant.Outlined;

    protected override void OnInitialized()
    {
        var page = NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty);

        switch (page)
        {
            case "tasks":
                SwitchToTasks();
                break;

            default:
                SwitchToCalendar();
                break;
        }
    }

    private void SwitchToCalendar()
    {
        _calendarVariant = Variant.Filled;
        _listVariant = Variant.Outlined;
    }

    private void SwitchToTasks()
    {
        _calendarVariant = Variant.Outlined;
        _listVariant = Variant.Filled;
    }

    private void NavigateToCalendar()
    {
        SwitchToCalendar();
        NavigationManager.NavigateTo("/");
    }

    private void NavigateToTasks()
    {
        SwitchToTasks();
        NavigationManager.NavigateTo("/tasks");
    }
}